// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Initialization code for select2

$('select').select2();
$('#CompatabileDevicesIds').select2({
    placeholder: 'Select devices',
    allowClear: true
});

$('#CategoryId').select2({
    placeholder: 'Select a category',
    allowClear: true
});

// End Initialization code for select2