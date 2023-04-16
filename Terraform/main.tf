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
    subnets         = [aws_subnet.operation_stacked_subnet_1.id, aws_subnet.operation_stacked_subnet_2.id]
    security_groups = [aws_security_group.ecs_security_group.id]
    assign_public_ip = "true"
  }

  load_balancer {
  target_group_arn = aws_lb_target_group.operation_stacked_tg.arn
  container_name   = "operation-stacked-api"
  container_port   = 80
}
}

data "aws_ssm_parameter" "operationstacked_db_password" {
  name = "operationstacked-dbpassword"
}

data "aws_ssm_parameter" "operationstacked_connection_string" {
  name = "operationstacked-connectionstring"
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

resource "aws_lb_target_group" "operation_stacked_tg" {
  name    = "operation-stacked-tg"
  port     = 80
  protocol = "HTTP"
  vpc_id   = aws_vpc.operation_stacked_vpc.id
  target_type = "ip" # Add this line
}

resource "aws_subnet" "operation_stacked_subnet_1" {
  vpc_id                  = aws_vpc.operation_stacked_vpc.id
  cidr_block              = "10.0.1.0/24"
  availability_zone       = "eu-west-1a" # Replace with the desired Availability Zone
}

resource "aws_subnet" "operation_stacked_subnet_2" {
  vpc_id                  = aws_vpc.operation_stacked_vpc.id
  cidr_block              = "10.0.2.0/24"
  availability_zone       = "eu-west-1b" # Replace with a different desired Availability Zone
}

resource "aws_lb" "operation_stacked_alb" {
  name               = "operation-stacked-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.ecs_security_group.id]

  subnets = [
    aws_subnet.operation_stacked_subnet_1.id,
    aws_subnet.operation_stacked_subnet_2.id
  ]
}
resource "aws_lb_listener" "operation_stacked_listener" {
  load_balancer_arn = aws_lb.operation_stacked_alb.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.operation_stacked_tg.arn
  }
}

output "alb_dns_name" {
  value = aws_lb.operation_stacked_alb.dns_name
}

resource "aws_internet_gateway" "operation_stacked_igw" {
  vpc_id = aws_vpc.operation_stacked_vpc.id
}

resource "aws_route_table" "operation_stacked_route_table" {
  vpc_id = aws_vpc.operation_stacked_vpc.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.operation_stacked_igw.id
  }
}

resource "aws_route_table_association" "operation_stacked_subnet_1_association" {
  subnet_id      = aws_subnet.operation_stacked_subnet_1.id
  route_table_id = aws_route_table.operation_stacked_route_table.id
}

resource "aws_route_table_association" "operation_stacked_subnet_2_association" {
  subnet_id      = aws_subnet.operation_stacked_subnet_2.id
  route_table_id = aws_route_table.operation_stacked_route_table.id
}