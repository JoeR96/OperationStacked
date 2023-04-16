provider "aws" {
  region = "eu-west-1" # Replace with your desired AWS region
}

variable "ecr_image_uri" {
  description = "ECR image URI for operation_stacked_api"
}

resource "aws_ecs_cluster" "operation_stacked_api" {
  name = "operation-stacked-api"
}
resource "aws_vpc" "operation_stacked_vpc" {
  cidr_block = "10.0.0.0/16"
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

  network_configuration {
    subnets          = [aws_subnet.operation_stacked_subnet.id]
    assign_public_ip = "ENABLED"
    security_groups  = [aws_security_group.ecs_security_group.id]
  }
}


resource "aws_subnet" "operation_stacked_subnet" {
  vpc_id     = aws_vpc.operation_stacked_vpc.id
  cidr_block = "10.0.0.0/24"
}



resource "aws_iam_role" "execution_role" {
  name_prefix = "ecs_execution_role"

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
  name_prefix = "ecs_task_role"

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
    subnets         = [aws_subnet.operation_stacked_subnet.id]
    security_groups = [aws_security_group.ecs_security_group.id]
  }
}


data "aws_ssm_parameter" "operationstacked_db_password" {
  name = "operationstacked-dbpassword"
}

data "aws_ssm_parameter" "operationstacked_connection_string" {
  name = "operationstacked-connectionstring"
}

data "aws_security_group" "existing_ecs_sg" {
  filter {
    name   = "tag:Name"
    values = ["ecs-sg*"]
  }
}


resource "aws_security_group" "ecs_security_group" {
  vpc_id      = aws_vpc.operation_stacked_vpc.id

  ingress {
    from_port   = 0
    to_port     = 65535
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  egress {
    from_port   = 0
    to_port     = 65535
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
}


