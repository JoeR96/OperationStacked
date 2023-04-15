resource "aws_db_instance" "operationstacked_db" {
  allocated_storage = 20 # 20 GB of General Purpose (SSD) storage for free tier
  engine            = "mysql"
  engine_version    = "8.0"
  instance_class    = "db.t2.micro" # Free tier eligible instance type

  name     = "OperationStacked"
  username = "operationstacked"
  password = "your_password"
  parameter_group_name = "default.mysql8.0"

  vpc_security_group_ids = [aws_security_group.operationstacked_db_sg.id]
  db_subnet_group_name   = aws_db_subnet_group.operationstacked_db_subnet_group.name

  backup_retention_period = 7
  multi_az               = false
  storage_encrypted      = true

  deletion_protection = false
  skip_final_snapshot = true
}

resource "aws_ssm_parameter" "operationstacked_db_password" {
  name  = "/operationstacked/dbpassword/"
  type  = "SecureString"
  value = "your_secure_password"
}

resource "aws_ssm_parameter" "operationstacked_connection_string" {
  name  = "/operationstacked/connectionstring/"
  type  = "String"
  value = "server=${aws_db_instance.operationstacked_db.endpoint};Port=3306;Database=OperationStacked;User Id=operationstacked;Password=${aws_ssm_parameter.operationstacked_db_password.value};"
}
