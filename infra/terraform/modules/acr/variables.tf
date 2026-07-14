# ./modules/acr/variables.tf

variable "resource_group_name" {
  description = "The name of the Azure resource group."
  type        = string
}

variable "location" {
  description = "The Azure region where the resources will be deployed."
  type        = string
}

variable "azure_container_registry_name" {
  description = "The name of the Azure Container Registry."
  type        = string
}
