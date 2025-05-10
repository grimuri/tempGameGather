output "webapp_name" {
  value = azurerm_linux_web_app.as.name
  description = "Nazwa aplikacji webowej"
}

output "webapp_publish_url" {
  value = "https://${azurerm_linux_web_app.as.default_hostname}/swagger/index.html"
  description = "URL publikacji aplikacji webowej"
}

output "resource_group_name" {
  value = azurerm_resource_group.rg.name
  description = "Nazwa grupy zasobów"
}

