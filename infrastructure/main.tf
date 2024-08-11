resource "azurerm_resource_group" "telemetry_rg" {
  location = var.location
  name     = "rg-telemetry-app-${var.environment}"
}