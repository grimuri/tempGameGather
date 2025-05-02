resource "random_string" "random_suffix" {
  length           = 4
  special          = false
  upper            = false
}

variable "prefix_api_base" {
  default = "preview-gamegather-api"
}

variable "prefix_db_base" {
  default = "preview-gamegather-db"
}

variable "location" {
  default = "polandcentral"
  
}

locals {
  prefix_api = "${var.prefix_api_base}-${random_string.random_suffix.result}"
  prefix_db  = "${var.prefix_db_base}-${random_string.random_suffix.result}"
}