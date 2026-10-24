using System.Text.RegularExpressions;

namespace TAREA.views;

public partial class inicioform1 : ContentPage
{
    public inicioform1()
    {
        InitializeComponent();
    }

    private async void OnEnviarInscripcionClicked(object sender, EventArgs e)
    {
        // 1. Obtener los valores ingresados
        string numId = txtNumeroId.Text?.Trim() ?? "";
        string apellidoPaterno = txtApellidoPaterno.Text?.Trim() ?? "";
        string apellidoMaterno = txtApellidoMaterno.Text?.Trim() ?? "";
        string nombres = txtNombres.Text?.Trim() ?? "";
        string telefono = txtTelefono.Text?.Trim() ?? "";
        string correo = txtCorreo.Text?.Trim() ?? "";
        string confirmarCorreo = txtConfirmarCorreo.Text?.Trim() ?? "";

        // VALIDACIÓN 1: Solo Números (Cédula/Número ID y Teléfono)
        if (!Regex.IsMatch(numId, @"^[0-9]+$"))
        {
            await DisplayAlert("Error de Validación", "El número de identificación debe contener únicamente números.", "OK");
            return;
        }

        if (!Regex.IsMatch(telefono, @"^[0-9]+$"))
        {
            await DisplayAlert("Error de Validación", "El teléfono debe contener únicamente números.", "OK");
            return;
        }

        // VALIDACIÓN 2: Solo Letras (Nombres y Apellidos, permite espacios y tildes)
        string patronLetras = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$";

        if (!Regex.IsMatch(apellidoPaterno, patronLetras) || !Regex.IsMatch(apellidoMaterno, patronLetras))
        {
            await DisplayAlert("Error de Validación", "Los apellidos deben contener únicamente letras.", "OK");
            return;
        }

        if (!Regex.IsMatch(nombres, patronLetras))
        {
            await DisplayAlert("Error de Validación", "Los nombres deben contener únicamente letras.", "OK");
            return;
        }

        // VALIDACIÓN 3: Coincidencia de correos
        if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(confirmarCorreo))
        {
            await DisplayAlert("Error de Validación", "Por favor, ingrese el correo personal y su confirmación.", "OK");
            return;
        }

        if (!correo.Equals(confirmarCorreo, StringComparison.OrdinalIgnoreCase))
        {
            await DisplayAlert("Error de Validación", "Los correos electrónicos no coinciden.", "OK");
            return;
        }

        // 4. GUARDAR EN ARCHIVO TXT
        try
        {
            string contenido = $"================ FORMULARIO DE REGISTRO ================\n" +
                               $"Fecha de Registro: {DateTime.Now}\n" +
                               $"Tipo ID: {pickerTipoId.SelectedItem}\n" +
                               $"Número ID: {numId}\n" +
                               $"Apellidos: {apellidoPaterno} {apellidoMaterno}\n" +
                               $"Nombres: {nombres}\n" +
                               $"Teléfono: {telefono}\n" +
                               $"Correo: {correo}\n" +
                               $"Carrera: {pickerCarrera.SelectedItem}\n" +
                               $"Modalidad: {pickerModalidad.SelectedItem}\n" +
                               $"Campus: {pickerCampus.SelectedItem}\n" +
                               $"========================================================\n\n";

            // Ruta de almacenamiento local de la aplicación
            string rutaArchivo = Path.Combine(FileSystem.AppDataDirectory, "registro_inscripcion.txt");

            // Escribir/Anexar la información en el archivo de texto
            File.AppendAllText(rutaArchivo, contenido);

            await DisplayAlert("Éxito", $"Inscripción registrada correctamente.\n\nArchivo guardado en:\n{rutaArchivo}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al guardar el archivo: {ex.Message}", "OK");
        }
    }
}