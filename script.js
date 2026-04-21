// ==================== CONFIGURACIÓN ====================
// ¡¡Cambia el puerto por el que te aparece a ti!!
const API_URL = "http://localhost:7933/api/Usuarios";  

// ==================== CARGAR TODOS LOS USUARIOS ====================
async function cargarUsuarios() {
    try {
        const response = await fetch(API_URL);
        if (!response.ok) throw new Error("Error al cargar usuarios");

        const usuarios = await response.json();

        const tbody = document.querySelector("#tablaUsuarios tbody");
        tbody.innerHTML = "";

        usuarios.forEach(u => {
            const fila = document.createElement("tr");
            fila.innerHTML = `
                <td>${u.id}</td>
                <td>${u.nombre}</td>
                <td>${u.telefono}</td>
                <td>${u.celular || '-'}</td>
                <td>${u.email || '-'}</td>
            `;
            tbody.appendChild(fila);
        });

    } catch (error) {
        console.error("Error cargando usuarios:", error);
        alert("No se pudieron cargar los usuarios. Revisa la consola (F12)");
    }
}

// ==================== CREAR NUEVO USUARIO ====================
async function crearUsuario(event) {
    event.preventDefault();

    const nuevoUsuario = {
        nombre:   document.getElementById("nombre").value.trim(),
        telefono: document.getElementById("telefono").value.trim(),
        celular:  document.getElementById("celular").value.trim() || null,
        email:    document.getElementById("email").value.trim() || null
    };

    try {
        const response = await fetch(API_URL, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(nuevoUsuario)
        });

        const resultado = await response.json();

        if (!response.ok) {
            throw new Error(resultado.error || "Error al guardar");
        }

        alert("✅ Usuario guardado correctamente");
        document.getElementById("formUsuario").reset();
        cargarUsuarios();        // recargar la tabla

    } catch (error) {
        console.error("Error:", error);
        alert("Error al guardar el usuario.\nRevisa la consola (F12) para más detalles");
    }
}

// ==================== INICIAR ====================
window.onload = function() {
    cargarUsuarios();

    const form = document.getElementById("formUsuario");
    form.addEventListener("submit", crearUsuario);
};