# 💰 API de Préstamos Personales

Sistema backend desarrollado con **Node.js + Express** y base de datos en **SQL Server**, diseñado para gestionar clientes, préstamos, cuotas, pagos parciales y moras.

---

## 🧠 Funcionalidades

- ✅ Registro de clientes
- ✅ Creación de préstamos
- ✅ Generación automática de cuotas
- ✅ Consulta del estado del préstamo (pagado, pendiente, parcial, mora)
- 🔄 Registro de pagos (completo y parcial)
- 📅 Cálculo de mora automática por retrasos
- 🛠️ Estructura preparada para escalar a app móvil (Flutter)

---

## 🚀 Tecnologías

| Tecnología     | Uso                             |
|----------------|----------------------------------|
| Node.js        | Backend                         |
| Express        | Framework API REST              |
| SQL Server     | Base de datos relacional         |
| Stored Procedures | Lógica de negocio (SP)       |
| Postman        | Pruebas locales de la API       |
| Git + GitHub   | Control de versiones y respaldo |

---

## 📂 Estructura del proyecto

│
├── database/ → Scripts SQL de tablas y SP
├── controllers/ → (opcional) Lógica de endpoints
├── routes/ → (opcional) Rutas separadas
├── index.js → Inicio de la aplicación
├── .env → Variables sensibles (conexión DB)
├── README.md → Descripción del proyecto
└── package.json → Dependencias y scripts


---

## 📬 Endpoints principales

| Método | Endpoint                         | Descripción                       |
|--------|----------------------------------|-----------------------------------|
| POST   | `/clientes`                      | Crear un nuevo cliente            |
| POST   | `/prestamos`                     | Crear préstamo y generar cuotas   |
| GET    | `/prestamos/:id/estado`          | Consultar estado de un préstamo   |
| POST   | `/pagos`                         | Registrar pago parcial o completo |

---

## ⚙️ Configuración local

1. Cloná el repositorio:

```bash
git clone https://github.com/jmendieta2710/api-prestamos.git
cd api-prestamos
npm install

DB_USER=Arosman
DB_PASSWORD=Aislinn.Db
DB_SERVER=74.208.122.227
DB_DATABASE=PrestamosDB
PORT=3000

node index.js


👨‍💻 Autor
Arosman Mendieta
📧 jmendieta2710@gmail.com
💼 GitHub

📄 Licencia
MIT License – Libre de usar, compartir y mejorar.

yaml
Copiar
Editar

---

## ✅ Próximo paso

1. Guardá este contenido como `README.md` en la raíz de tu proyecto.
2. Hacé un nuevo commit para subirlo:

```bash
git add README.md
git commit -m "Agregando README profesional al proyecto"
git push

EJEMPLO

![image](https://github.com/user-attachments/assets/39e12ef4-3190-463d-801c-296b83770234)

