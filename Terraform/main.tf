provider "aws" {
  region                  = "eu-west-1" # Replace with your desired AWS region
  access_key              = var.aws_access_key_id
  secret_key              = var.aws_secret_access_key
}

variable "aws_access_key_id" {
  default = ""
}

variable "aws_secret_access_key" {
  default = ""
}


resource "aws_lambda_function" "dotnet_api" {
  function_name = "DotnetApiFunction"

  filename = "dotnet-api.zip"
  source_code_hash = filebase64sha256("dotnet-api.zip")

  handler = "DotnetApi::DotnetApi.LambdaEntryPoint::FunctionHandlerAsync"
  runtime = "dotnet6" # Use the appropriate .NET runtime version

  role = aws_iam_role.lambda_exec.arn

  environment {
    variables = {
      CONNECTION_STRING = data.aws_ssm_parameter.operationstacked_connection_string.value
    }
  }
}

# Destroy existing Lambda function
resource "aws_lambda_function" "dotnet_api_deletion" {
  function_name = "DotnetApiFunction"
    role = aws_iam_role.lambda_exec.arn

  # Ensure that Terraform will delete the function
  lifecycle {
    ignore_changes = [source_code_hash]
    create_before_destroy = true
  }

  # Do not specify any other attributes to avoid potential conflicts with the existing function
}

# Create a new Lambda function with the same name and updated details
resource "aws_lambda_function" "dotnet_api_creation" {
  function_name = "DotnetApiFunction"

  filename = "updated-dotnet-api.zip"
  source_code_hash = filebase64sha256("updated-dotnet-api.zip")

  handler = "DotnetApi::DotnetApi.LambdaEntryPoint::FunctionHandlerAsync"
  runtime = "dotnet6"

  role = aws_iam_role.lambda_exec.arn

  environment {
    variables = {
      CONNECTION_STRING = data.aws_ssm_parameter.updated_operationstacked_connection_string.value
    }
  }
}


resource "aws_iam_role" "lambda_exec" {
  name = "lambda_exec"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "lambda.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "lambda_exec_policy" {
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
  role       = aws_iam_role.lambda_exec.name
}

resource "aws_api_gateway_rest_api" "dotnet_api" {
  name = "DotnetApi"
}

resource "aws_api_gateway_resource" "dotnet_resource" {
  rest_api_id = aws_api_gateway_rest_api.dotnet_api.id
  parent_id   = aws_api_gateway_rest_api.dotnet_api.root_resource_id
  path_part   = "{proxy+}"
}

resource "aws_api_gateway_method" "dotnet_proxy" {
  rest_api_id   = aws_api_gateway_rest_api.dotnet_api.id
  resource_id   = aws_api_gateway_resource.dotnet_resource.id
  http_method   = "ANY"
  authorization = "NONE"
}

resource "aws_api_gateway_integration" "dotnet_lambda" {
  rest_api_id             = aws_api_gateway_rest_api.dotnet_api.id
  resource_id             = aws_api_gateway_resource.dotnet_resource.id
  http_method             = aws_api_gateway_method.dotnet_proxy.http_method
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.dotnet_api.invoke_arn
}

resource "aws_api_gateway_deployment" "dotnet_api_deployment" {
  depends_on = [aws_api_gateway_integration.dotnet_lambda]

  rest_api_id = aws_api_gateway_rest_api.dotnet_api.id
  stage_name  = "prod"
}

output "api_url" {
  value = "${aws_api_gateway_deployment.dotnet_api_deployment.invoke_url}{proxy}"
}

data "aws_ssm_parameter" "operationstacked_db_password" {
  name       = "operationstacked-dbpassword"
  depends_on = [aws_ssm_parameter.operationstacked_db_password]
}

data "aws_ssm_parameter" "operationstacked_connection_string" {
  name       = "operationstacked-connectionstring"
  depends_on = [aws_ssm_parameter.operationstacked_connection_string]
}

data "aws_ssm_parameter" "updated_operationstacked_connection_string" {
  name       = "updated-operationstacked-connectionstring"
  depends_on = [aws_ssm_parameter.operationstacked_connection_string]
}
