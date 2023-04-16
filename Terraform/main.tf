provider "aws" {
  region     = "eu-west-1" # Replace with your desired AWS region
  access_key = var.aws_access_key_id
  secret_key = var.aws_secret_access_key
}

data "aws_vpc" "existing_operation_stacked_vpc" {
  filter {
    name   = "tag:Name"
    values = ["OperationStackedVPC"]
  }

  optional = true
}

resource "aws_vpc" "operation_stacked_vpc" {
  count = length(local.existing_vpc_id) > 0 ? 0 : 1

  cidr_block = "10.0.0.0/16"

  tags = {
    Name = "OperationStackedVPC"
  }
}

resource "aws_subnet" "operation_stacked_subnet" {
  count = 2

  cidr_block = "10.0.${count.index + 1}.0/24"
  vpc_id     = aws_vpc.operation_stacked_vpc[0].id

  tags = {
    Name = "OperationStackedSubnet-${count.index + 1}"
  }
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

resource "aws_ecs_cluster" "operation_stacked_api" {
  name = "operation-stacked-api"
}

resource "aws_ecs_task_definition" "operation_stacked_api" {
  family                   = "operation-stacked-api"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = local.execution_role_arn
  task_role_arn            = local.task_role_arn

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

data "aws_iam_role" "existing_execution_role" {
  name = "ecs_execution_role"
}

resource "aws_iam_role" "execution_role" {
  count = data.aws_iam_role.existing_execution_role.id != null ? 0 : 1

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

data "aws_iam_role" "existing_task_role" {
  name = "ecs_task_role"
}

resource "aws_iam_role" "task_role" {
  count = data.aws_iam_role.existing_task_role.id != null ? 0 : 1

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
locals {
  execution_role_arn = data.aws_iam_role.existing_execution_role.id != null ? data.aws_iam_role.existing_execution_role.arn : aws_iam_role.execution_role[0].arn
  task_role_arn = data.aws_iam_role.existing_task_role.id != null ? data.aws_iam_role.existing_task_role.arn : aws_iam_role.task_role[0].arn
  existing_vpc_id = try(data.aws_vpc.existing_operation_stacked_vpc.id, "")
}

resource "aws_iam_role_policy_attachment" "ecs_execution_policy" {
  count      = length(aws_iam_role.execution_role)
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
  role       = aws_iam_role.execution_role[count.index].name
}

resource "aws_ecs_service" "operation_stacked_api" {
  name            = "operation-stacked-api"
  cluster         = aws_ecs_cluster.operation_stacked_api.id
  task_definition = aws_ecs_task_definition.operation_stacked_api.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    subnets = aws_subnet.operation_stacked_subnet.*.id
  }
}

data "aws_ssm_parameter" "operationstacked_db_password" {
  name = "operationstacked-dbpassword"
}

data "aws_ssm_parameter" "operationstacked_connection_string" {
  name = "operationstacked-connectionstring"
}
