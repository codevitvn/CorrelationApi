# CorrelationApi

This is a demonstration using correlationId to trace log.

The request firstly comes to company api, then the LogHeaderMiddleware will insert the correlation id into the context.items by getting from the header or generating new one
The weatherforcast action will use CorrelationIdAccessor to get the correlationId from context.items
The weatherforcast action will raise a request to the product api to get the needed information. Send the correlation id along with the request in the header.

At the product api, will get the correlation id from the header, then send a command to the MassTransit Bus which is connected to RabbitMQ. The Correlation id is included in the message
The Dashboard console app will handle the command that sends from the product api, the consumer can extract the correlation id from the message for logging.

That's all about this system, it can log every related information by using correlation id.

#AspNetCore, #Serilog, #MassTransit, #RabbitMQ
