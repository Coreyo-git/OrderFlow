resource "azurerm_container_registry" "acr" {
  name                = var.azure_container_registry_name
  resource_group_name = var.resource_group_name
  location            = var.location
  sku                 = "Basic"
  admin_enabled       = false
  #   georeplications {
  #     location                = var.location
  #     zone_redundancy_enabled = true
  #     tags                    = {}
  #   }
}