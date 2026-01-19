$.validator.addMethod('fileSizeValidator', function (uploadedValue, htmlElement, maxSize) {
    return htmlElement.files[0].size <= maxSize;
});