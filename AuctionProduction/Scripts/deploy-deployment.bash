az login

#Deploy services/nginx
az container create -g FantasticFour --file deploy-services-aci.yaml