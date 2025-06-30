// site.js - Versión 1.1 (Revisado)
// Funciones JavaScript del sitio

// Inicialización cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', function () {
    initializeTooltips();
    initializeAlerts();
    initializeFormValidation();
    initializeDynamicSearch();
});

// Inicializar tooltips de Bootstrap
function initializeTooltips() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Auto-cerrar alertas después de un tiempo
function initializeAlerts() {
    const alerts = document.querySelectorAll('.alert:not(.alert-permanent)');
    alerts.forEach(function (alert) {
        if (alert.classList.contains('alert-success') || alert.classList.contains('alert-info')) {
            setTimeout(function () {
                var bsAlert = new bootstrap.Alert(alert);
                bsAlert.close();
            }, 5000); // 5 segundos
        }
    });
}

// Mejorar validación de formularios
function initializeFormValidation() {
    // Agregar clases de Bootstrap a campos de validación
    const forms = document.querySelectorAll('form');
    forms.forEach(function (form) {
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        });

        // Validación en tiempo real
        const inputs = form.querySelectorAll('input, textarea, select');
        inputs.forEach(function (input) {
            input.addEventListener('blur', function () {
                if (input.checkValidity()) {
                    input.classList.remove('is-invalid');
                    input.classList.add('is-valid');
                } else {
                    input.classList.remove('is-valid');
                    input.classList.add('is-invalid');
                }
            });
        });
    });
}

// Búsqueda dinámica mejorada
function initializeDynamicSearch() {
    const searchInputs = document.querySelectorAll('input[type="search"], input[id*="search"]');
    searchInputs.forEach(function (input) {
        let searchTimeout;
        input.addEventListener('input', function (e) {
            clearTimeout(searchTimeout);
            const form = e.target.closest('form');

            if (form) {
                searchTimeout = setTimeout(function () {
                    if (e.target.value.length >= 2 || e.target.value.length === 0) {
                        form.submit();
                    }
                }, 500);
            }
        });
    });
}

// Funciones utilitarias

// Confirmar acción de eliminación
function confirmDelete(itemName) {
    return confirm(`¿Estás seguro de que quieres eliminar "${itemName}"? Esta acción no se puede deshacer.`);
}

// Mostrar mensaje de carga en botones
function showButtonLoading(button, text = 'Procesando...') {
    if (button) {
        button.originalText = button.innerHTML;
        button.innerHTML = `<i class="fas fa-spinner fa-spin"></i> ${text}`;
        button.disabled = true;
        button.classList.add('btn-loading');
    }
}

// Ocultar mensaje de carga en botones
function hideButtonLoading(button) {
    if (button && button.originalText) {
        button.innerHTML = button.originalText;
        button.disabled = false;
        button.classList.remove('btn-loading');
    }
}

// Formatear números como moneda
function formatCurrency(amount, currency = 'USD') {
    return new Intl.NumberFormat('es-ES', {
        style: 'currency',
        currency: currency
    }).format(amount);
}

// Formatear fechas
function formatDate(date, locale = 'es-ES') {
    return new Date(date).toLocaleDateString(locale, {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
}

// Validar formato de email
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// Copiar texto al portapapeles
async function copyToClipboard(text) {
    try {
        await navigator.clipboard.writeText(text);
        showToast('Copiado al portapapeles', 'success');
    } catch (err) {
        console.error('Error al copiar: ', err);
        showToast('Error al copiar al portapapeles', 'danger');
    }
}

// Mostrar notificaciones toast
function showToast(message, type = 'info', duration = 3000) {
    const toastContainer = document.getElementById('toast-container') || createToastContainer();

    const toast = document.createElement('div');
    toast.className = `toast align-items-center text-white bg-${type} border-0`;
    toast.setAttribute('role', 'alert');
    toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                ${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
        </div>
    `;

    toastContainer.appendChild(toast);

    const bsToast = new bootstrap.Toast(toast, { delay: duration });
    bsToast.show();

    toast.addEventListener('hidden.bs.toast', function () {
        toast.remove();
    });
}

// Crear contenedor de toasts si no existe
function createToastContainer() {
    const container = document.createElement('div');
    container.id = 'toast-container';
    container.className = 'toast-container position-fixed top-0 end-0 p-3';
    container.style.zIndex = '1055';
    document.body.appendChild(container);
    return container;
}

// Manejar errores AJAX globales
window.addEventListener('unhandledrejection', function (event) {
    console.error('Error no manejado:', event.reason);
    showToast('Ha ocurrido un error inesperado', 'danger');
});

// Funciones para trabajar con el localStorage de forma segura
const SafeStorage = {
    set: function (key, value) {
        try {
            localStorage.setItem(key, JSON.stringify(value));
            return true;
        } catch (e) {
            console.warn('No se pudo guardar en localStorage:', e);
            return false;
        }
    },

    get: function (key, defaultValue = null) {
        try {
            const item = localStorage.getItem(key);
            return item ? JSON.parse(item) : defaultValue;
        } catch (e) {
            console.warn('No se pudo leer de localStorage:', e);
            return defaultValue;
        }
    },

    remove: function (key) {
        try {
            localStorage.removeItem(key);
            return true;
        } catch (e) {
            console.warn('No se pudo eliminar de localStorage:', e);
            return false;
        }
    }
};

// Exportar funciones para uso global
window.InventoryApp = {
    confirmDelete,
    showButtonLoading,
    hideButtonLoading,
    formatCurrency,
    formatDate,
    isValidEmail,
    copyToClipboard,
    showToast,
    SafeStorage
};
