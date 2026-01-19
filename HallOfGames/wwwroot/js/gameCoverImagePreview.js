$(document).ready(function () {
    $('#Cover').on('change', function () {
        $('#uploaded-image').attr('src', window.URL.createObjectURL(this.files[0]));
    });
});