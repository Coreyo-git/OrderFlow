

# Generate a random integer to create a globally unique name
resource "random_integer" "ri" {
  min = 10000
  max = 99999
}

# 2. Combine the variable and the random number into a single name
locals {
  final_resource_group_name = "${var.resource_group_name}-${random_integer.ri.result}"
  final_acr_name            = "${var.azure_container_registry_name}${random_integer.ri.result}"
}

# Create the resource group
resource "azurerm_resource_group" "orderflow-rg" {
  name     = local.final_resource_group_name
  location = var.location
}

module "acr" {
  source                        = "./modules/acr"
  azure_container_registry_name = local.final_acr_name
  resource_group_name           = local.final_resource_group_name
  location                      = var.location
}