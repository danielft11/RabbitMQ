# RabbitMQ

## Objetivo:
Este projeto tem como objetivo o estudo de mensageria com RabbitMQ e será incrementado no futuro com a biblioteca MassTransit e outras tecnologias relacionadas a mensageria.

## Instruções de utilização:
Você precisa ter o docker instalado em sua máquina. Caso ainda não possua faça a instalação seguindo este tutorial https://docs.docker.com/desktop/install/windows-install/.

1. Com o docker instalado abra o Windows PowerShell e navegue até a pasta onde foi feito o download do projeto, por exemplo: D:\Desenvolvimento\Projetos Visual Studio\RabbitMQ
   
2. Execute o comando "docker compose up -d". Este comando irá baixar a imagem do RabbitMQ com as configurações definidas no arquivo **docker-compose.yml** e executar o container.
 
3. Assim que o container estiver em execução, abra o navegador de sua escolha e acesse o Painel do docker através do endereço **http://localhost:15672**. Digite **guest** no campo de usuário e senha para logar.
   
4. No Visual Studio (ou outra IDE de sua escolha) clique com o botão direito sobre a solution RabbitMQ e, em seguida, clique em Properties. Selecione a opção **Multiple Startup Project** e escolha a opção **Start** na coluna Action para os projetos
*WebApplicationConsumer* e *WebApplicationPublish*.



