resource "azurerm_cosmosdb_account" "db" {
  name                = "cosdb-telemetry-${var.environment}"
  location            = var.location
  resource_group_name = azurerm_resource_group.telemetry_rg.name
  offer_type          = "Standard"
  kind                = "GlobalDocumentDB"
  free_tier_enabled   = true

  consistency_policy {
    consistency_level = "Session"
  }
  geo_location {
    failover_priority = 0
    location          = var.location
  }
}