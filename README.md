# CargoPay Authorization System

## Descripción del Proyecto

El **CargoPay Authorization System** es un sistema de autorización y gestión de pagos desarrollado con .NET Core, diseñado para manejar transacciones de manera eficiente, segura y escalable. Aunque originalmente fue desarrollado para la gestión de tarjetas y pagos en un entorno empresarial, su arquitectura y funcionalidades lo hacen ideal para integrarse con plataformas de comercio electrónico como Shopify.

## Características Principales

- **Gestión de Tarjetas:**
  - Generación de tarjetas virtuales con saldo inicial.
  - Consultas de saldo en tiempo real.
  - Procesamiento de pagos con cálculo automático de tarifas.

- **Manejo de Transacciones:**
  - Las transacciones son seguras y se procesan mediante un sistema de tarifas dinámicas proporcionadas por el servicio Universal Fees Exchange (UFE).
  - El sistema actualiza las tarifas automáticamente cada hora.

- **Escalabilidad:**
  - Arquitectura basada en principios de diseño modular (MVC) que permite su integración con otros sistemas, como APIs externas para tiendas en línea.

- **Documentación Interactiva:**
  - Uso de Swagger para documentar y probar los endpoints disponibles.

## Relación con e-Commerce (Shopify)

Este sistema puede complementarse perfectamente con una tienda en Shopify debido a las siguientes razones:

1. **Integración API para Pagos:**
   - Los endpoints existentes (como creación de tarjeta, consulta de saldo y procesamiento de pagos) pueden ser integrados directamente con la API de Shopify para gestionar pagos de clientes.

2. **Gestión Escalable:**
   - Escalable para manejar un gran volumen de transacciones diarias, ideal para tiendas en línea con alto tráfico.

3. **Cálculo Dinámico de Tarifas:**
   - La funcionalidad de cálculo dinámico de tarifas puede ser utilizada para definir costos personalizados en transacciones específicas, una necesidad común en e-commerce.

4. **Seguridad:**
   - Autenticación mediante JWT (JSON Web Tokens) para proteger la información del cliente y las operaciones realizadas.

## Tecnologías Utilizadas

- **Backend:**
  - .NET Core
  - Entity Framework Core
  - SQL Server

- **Patrones de Diseño:**
  - MVC (Model-View-Controller)
  - Dependency Injection
  - Servicios en segundo plano para tareas recurrentes (actualización de tarifas).

- **Seguridad:**
  - Autenticación con JWT
  - Middleware personalizado para manejo de excepciones globales.

- **Documentación:**
  - Swagger para documentación de API.

## Endpoints Clave

1. **Crear Tarjeta**
   - URL: `/api/Card`
   - Método: `POST`
   - Descripción: Crea una nueva tarjeta con un saldo inicial.

2. **Procesar Pago**
   - URL: `/api/Card/{id}/pay`
   - Método: `POST`
   - Descripción: Realiza un pago usando una tarjeta existente.

3. **Consultar Saldo**
   - URL: `/api/Card/{id}/balance`
   - Método: `GET`
   - Descripción: Recupera el saldo actual de la tarjeta.

## Cómo Ejecutarlo Localmente

### Requisitos Previos
1. Instalar **.NET Core SDK**.
2. Instalar **SQL Server**.
3. Tener configurado **Entity Framework Core** para la base de datos.

### Configuración del Proyecto
1. Clona el repositorio:
   ```bash
   git clone https://github.com/ingenieraLesly/CargoPayAuthorizationSystem.git
   ```
2. Configura tu cadena de conexión en el archivo `appsettings.json`.
3. Crea la base de datos ejecutando los scripts en el archivo `SETUP.md`.
4. Aplica las migraciones de Entity Framework:
   ```bash
   dotnet ef database update
   ```
5. Ejecuta la aplicación:
   ```bash
   dotnet run
   ```

### Ejecución en Producción
Este sistema puede desplegarse en plataformas como **Azure**, **Heroku** o **AWS**, y configurarse para manejar pagos de manera remota.

## Demo en Producción

Actualmente disponible en:  
[https://my-cargopay.azurewebsites.net](https://my-cargopay.azurewebsites.net) *(Si tienes esta URL funcional, inclúyela aquí)*.

## Contribuciones Futuras

Para integrar este sistema completamente con Shopify, se podrían desarrollar las siguientes mejoras:
1. Adaptar los endpoints para comunicarse directamente con la **Shopify Admin API**.
2. Personalizar la interfaz de usuario para gestionar pagos dentro del entorno de Shopify.
3. Escalar el sistema para soportar múltiples monedas y métodos de pago.

## Autor

Lesly Paola Aguilar Rincón  
Desarrolladora de Software | [GitHub](https://github.com/ingenieraLesly) | [LinkedIn](https://www.linkedin.com/in/lesly-flytric/)  

