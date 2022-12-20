az login
az container stop -g FantasticFour -n fantasticfourDevOpsGroup
az container stop -g FantasticFour -n fantasticfourServicesGroup
az container stop -g FantasticFour -n fantasticfourBackendGroup
az network application-gateway stop -g FantasticFour -n fantasticag