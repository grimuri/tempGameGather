# Database

# resource "azurerm_resource_group" "rg_db" {
#   name     = "${local.prefix_db}-rg"
#   location = var.location
# }

resource "azurerm_resource_group" "rg" {
  name     = "${local.prefix_api}-rg"
  location = var.location
}

resource "azurerm_postgresql_flexible_server" "postgresql_server" {
  name                   = "${local.prefix_db}-server"
  resource_group_name    = azurerm_resource_group.rg.name
  location               = azurerm_resource_group.rg.location
  version                = "16"
  administrator_login    = "postgres"
  administrator_password = "postgres"
  
  sku_name               = "B_Standard_B1ms"

  storage_mb             = 32768
  zone                   = "1"  

  backup_retention_days  = 7
  
  geo_redundant_backup_enabled = false
}

resource "azurerm_postgresql_flexible_server_database" "postgresql_db" {
  name      = "${local.prefix_db}-database"
  server_id = azurerm_postgresql_flexible_server.postgresql_server.id
  collation = "en_US.utf8"
  charset   = "UTF8"
}

resource "azurerm_postgresql_flexible_server_firewall_rule" "allow_azure_services" {
  name                = "AllowAzureServices"
  server_id           = azurerm_postgresql_flexible_server.postgresql_server.id
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}

resource "azurerm_postgresql_flexible_server_firewall_rule" "allow_me" {
  name                = "AllowMe"
  server_id           = azurerm_postgresql_flexible_server.postgresql_server.id
  start_ip_address    = "178.235.126.128"
  end_ip_address      = "178.235.126.128"
}

resource "azurerm_postgresql_flexible_server_configuration" "ssl_off" {
  name      = "require_secure_transport"
  server_id = azurerm_postgresql_flexible_server.postgresql_server.id
  value     = "off"
}


# Backend

resource "azurerm_service_plan" "asp" {
  name                = "${local.prefix_api}-asp"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "B1"
}

resource "azurerm_linux_web_app" "as" {
  name                = "${local.prefix_api}-webapp"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  service_plan_id     = azurerm_service_plan.asp.id

  site_config {
    always_on = false
    application_stack {
        dotnet_version = "8.0"
    }

    cors {
      allowed_origins = ["*"]
    }
  }

  app_settings = {
    "ConnectionStrings__Default" = "Server=${azurerm_postgresql_flexible_server.postgresql_server.fqdn};Port=5432;Database=${azurerm_postgresql_flexible_server_database.postgresql_db.name};Username=${azurerm_postgresql_flexible_server.postgresql_server.administrator_login};Password=${azurerm_postgresql_flexible_server.postgresql_server.administrator_password};SSL Mode=Require;"
  }

}