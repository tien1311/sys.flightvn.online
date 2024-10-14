document.getElementById('inputImage').addEventListener('change', function (event) {
    const file = event.target.files[0];
    const imageContainer = document.getElementById('image-container');

    // Clear any existing images
    imageContainer.innerHTML = '';

    if (file) {
        const reader = new FileReader();

        reader.onload = function (e) {
            const img = document.createElement('img');
            img.src = e.target.result;
            img.style.maxWidth = '100%'; // Adjust as needed
            img.style.height = 'auto';   // Adjust as needed
            imageContainer.appendChild(img);
        };

        reader.readAsDataURL(file);
    }
});