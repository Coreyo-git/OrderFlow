# OrderFlow Terraform

Manages the Azure infrastructure for OrderFlow. Currently provisions a resource group and an Azure Container Registry (ACR); more resources (AKS, managed Postgres, etc.) will be added as later roadmap phases are built out.

## Structure

```
infra/terraform/
  providers.tf      # required providers + azurerm provider config
  variables.tf       # root input variables (with defaults)
  main.tf             # resource group + module calls
  modules/
    acr/              # Azure Container Registry module
```

## Prerequisites

- [Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli), logged in via `az login`
- [Terraform](https://developer.hashicorp.com/terraform/install) >= 1.15.0

## Authentication

The `azurerm` provider requires an explicit subscription ID (as of provider v4) — it will not silently fall back to whatever subscription is active in your `az` session. Set it via an environment variable before running any Terraform command:

```bash
export ARM_SUBSCRIPTION_ID=$(az account show --query id -o tsv)
```

This is only set for your current terminal session. Add it to your shell profile (`~/.zshrc`, `~/.bashrc`) if you want it to persist across sessions.

## Usage

```bash
cd infra/terraform

terraform init      # downloads providers/modules
terraform plan       # preview changes — safe, makes no changes
terraform apply       # apply changes — prompts for confirmation
```

To tear down everything this config manages:

```bash
terraform destroy
```

## Notes

- State is stored locally (`terraform.tfstate`) and is gitignored — it is never committed, since it can contain sensitive values. This is fine for solo/learning use; a team or CI setup would need a remote state backend (e.g. an Azure Storage Account) instead.
- Resource names that must be globally unique across Azure (like the ACR name) get a random numeric suffix appended — see `main.tf`.
