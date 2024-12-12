#!/bin/sh

# set region globally
export AWS_DEFAULT_REGION=sa-east-1
export AWS_ACCESS_KEY_ID=teste
export AWS_SECRET_ACCESS_KEY=teste
export AWS_DEFAULT_OUTPUT=json

# install `jq`, if not yet available
which jq || apt-get -y install jq
sleep 5s

# export REST_API_ID=12345

# # create rest api gateway
# echo "Create Rest API..."
# aws --endpoint-url=http://localstack:4566 apigateway create-rest-api --name quote-api-gateway --tags '{"_custom_id_":"12345"}'

# # get parent id of resource
# echo "Export Parent ID..."
# export PARENT_ID=$(aws --endpoint-url=http://localstack:4566 apigateway get-resources --rest-api-id $REST_API_ID | jq -r '.items[0].id')pi

# # get resource id
# echo "Export Resource ID..."
# export RESOURCE_ID=$(aws --endpoint-url=http://localstack:4566 apigateway create-resource --rest-api-id $REST_API_ID --parent-id $PARENT_ID --path-part "productApi" | jq -r '.id')

# echo "RESOURCE ID: $RESOURCE_ID"

# echo "Put GET Method..."
# aws --endpoint-url=http://localstack:4566 apigateway put-method \
# --rest-api-id $REST_API_ID \
# --resource-id $RESOURCE_ID \
# --http-method GET \
# --request-parameters "method.request.path.productApi=true" \
# --authorization-type "NONE"

# echo "Put POST Method..."
# aws --endpoint-url=http://localstack:4566 apigateway put-method \
# --rest-api-id $REST_API_ID \
# --resource-id $RESOURCE_ID \
# --http-method POST \
# --request-parameters "method.request.path.productApi=true" \
# --authorization-type "NONE"


# echo "Update GET Method..."
# aws --endpoint-url=http://localstack:4566 apigateway update-method \
#   --rest-api-id $REST_API_ID \
#   --resource-id $RESOURCE_ID \
#   --http-method GET \
#   --patch-operations "op=replace,path=/requestParameters/method.request.querystring.param,value=true"


# echo "Put POST Method Integration..."
# aws --endpoint-url=http://localstack:4566 apigateway put-integration \
#   --rest-api-id $REST_API_ID \
#   --resource-id $RESOURCE_ID \
#   --http-method POST \
#   --type AWS_PROXY \
#   --integration-http-method POST \
#   --uri arn:aws:apigateway:$AWS_DEFAULT_REGION:lambda:path/2015-03-31/functions/arn:aws:lambda:$AWS_DEFAULT_REGION:000000000000:function:add-product/invocations \
#   --passthrough-behavior WHEN_NO_MATCH

# echo "Put GET Method Integration..."
# aws --endpoint-url=http://localstack:4566 apigateway put-integration \
#   --rest-api-id $REST_API_ID \
#   --resource-id $RESOURCE_ID \
#   --http-method GET \
#   --type AWS_PROXY \
#   --integration-http-method GET \
#   --uri arn:aws:apigateway:$AWS_DEFAULT_REGION:lambda:path/2015-03-31/functions/arn:aws:lambda:$AWS_DEFAULT_REGION:000000000000:function:get-product/invocations \
#   --passthrough-behavior WHEN_NO_MATCH

# echo "Create DEV Deployment..."
# aws --endpoint-url=http://localstack:4566 apigateway create-deployment \
#   --rest-api-id $REST_API_ID \
#   --stage-name dev


echo "Criar tópico"
aws --endpoint-url=http://localstack:4566 sns create-topic --name webhook-topic

echo "Criar fila DLQ"
aws --endpoint-url=http://localstack:4566 sqs create-queue  --queue-name websocket-queue-deadletter

echo "Criar Fila"
aws --endpoint-url=http://localstack:4566 sqs create-queue --queue-name webhook-queue --attributes file://create-queue.json

echo "Recuperar ARN da fila"
export QUEUE_ARN=$(aws --endpoint-url=http://localstack:4566 sqs get-queue-attributes --queue-url http://localhost:4566/000000000000/webhook-queue --attribute-names QueueArn | jq -r '.Attributes.QueueArn')
echo "QUEUE ARN: $QUEUE_ARN"

echo "Criar Subscription"
export SUBSCRIPTION_ARN=$(aws --endpoint-url=http://localstack:4566 sns subscribe \
    --topic-arn arn:aws:sns:$AWS_DEFAULT_REGION:000000000000:webhook-topic \
    --protocol sqs \
    --notification-endpoint $QUEUE_ARN | jq -r '.SubscriptionArn')


echo "Subscription ARN: $SUBSCRIPTION_ARN"


echo "Setar atributo da subscription"
aws --endpoint-url=http://localstack:4566 sns set-subscription-attributes \
    --subscription-arn $SUBSCRIPTION_ARN \
    --attribute-name RawMessageDelivery \
    --attribute-value true