output "login_server" {
  value       = azurerm_container_registry.acr.login_server
  description = "The login server URL of the Azure Container Registry."
}
