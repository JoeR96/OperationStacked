provider "aws" {
  region     = "eu-west-1" # Replace with your desired AWS region
  access_key = var.aws_access_key_id
  secret_key = var.aws_secret_access_key
}

variable "aws_access_key_id" {
  default = ""
}

variable "aws_secret_access_key" {
  default = ""
}

variable "ecr_image_uri" {
  description = "ECR image URI for operation_stacked_api"
}

variable "subnets" {
  description = "List of subnet IDs for the ECS service"
  type        = list(string)
}

resource "aws_ecs_cluster" "operation_stacked_api" {
  name = "operation-stacked-api"
}

resource "aws_ecs_task_definition" "operation_stacked_api" {
  family                   = "operation-stacked-api"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.execution_role.arn
  task_role_arn            = aws_iam_role.task_role.arn

  container_definitions = jsonencode([{
    name  = "operation-stacked-api"
    image = var.ecr_image_uri
    portMappings = [
      {
        containerPort = 80
        hostPort      = 80
      }
    ]
    environment = [
      {
        name  = "CONNECTION_STRING"
        value = data.aws_ssm_parameter.operationstacked_connection_string.value
      }
    ]
  }])
}

resource "aws_iam_role" "execution_role" {
  name = "ecs_execution_role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "ecs-tasks.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_role" "task_role" {
  name = "ecs_task_role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "ecs-tasks.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "ecs_execution_policy" {
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
  role       = aws_iam_role.execution_role.name
}

resource "aws_ecs_service" "operation_stacked_api" {
  name            = "operation-stacked-api"
  cluster         = aws_ecs_cluster.operation_stacked_api.id
  task_definition = aws_ecs_task_definition.operation_stacked_api.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    subnets = var.subnets
  }
}

data "aws_ssm_parameter" "operationstacked_db_password" {
  name = "operationstacked-dbpassword"
  depends_on = [aws_ssm_parameter.operationstacked_db_password]
}

data "aws_ssm_parameter" "operationstacked_connection_string" {
  name = "operationstacked-connectionstring"
  depends_on = [aws_ssm_parameter.operationstacked_connection_string]
}
