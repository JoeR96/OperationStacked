resource "aws_db_instance" "operationstacked_db" {
  allocated_storage = 20 # 20 GB of General Purpose (SSD) storage for free tier
  engine            = "mysql"
  engine_version    = "8.0"
  instance_class    = "db.t2.micro" # Free tier eligible instance type

  name     = "OperationStacked"
  username = "operationstacked"
  password = "your_password"
  parameter_group_name = "default.mysql8.0"

  vpc_security_group_ids = [local.operationstacked_db_sg_id]

  backup_retention_period = 7
  multi_az               = false
  storage_encrypted      = false

  deletion_protection = false
  skip_final_snapshot = true
  lifecycle {
    create_before_destroy = true
  }
  publicly_accessible = true # Set this parameter to true to make the database publicly accessible
}

resource "aws_ssm_parameter" "operationstacked_db_password" {
  name  = "operationstacked-dbpassword"
  type  = "SecureString"
  value = "your_secure_password"
  overwrite = true
}

resource "aws_ssm_parameter" "operationstacked_connection_string" {
  name  = "operationstacked-connectionstring"
  type  = "String"
  value = "server=${aws_db_instance.operationstacked_db.endpoint};Port=3306;Database=OperationStacked;User Id=operationstacked;Password=${aws_ssm_parameter.operationstacked_db_password.value};"
  overwrite = true
}

data "aws_security_group" "existing_operationstacked_db_sg" {
  filter {
    name   = "group-name"
    values = ["operationstacked-db-sg"]
  }
}

resource "aws_security_group" "operationstacked_db_sg" {
  count       = data.aws_security_group.existing_operationstacked_db_sg.id != null ? 0 : 1
  name        = "operationstacked-db-sg"
  description = "Security group for OperationStacked RDS instance"

  ingress {
    from_port   = 3306
    to_port     = 3306
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"] # Adjust this to your desired IP range
  }
}

locals {
  operationstacked_db_sg_id = data.aws_security_group.existing_operationstacked_db_sg.id != null ? data.aws_security_group.existing_operationstacked_db_sg.id : aws_security_group.operationstacked_db_sg[0].id
}
