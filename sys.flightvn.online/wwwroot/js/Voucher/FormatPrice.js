
function formatNumberVnd(number) {
    let num = parseFloat(number.replace(/,/g, ''));
    if (isNaN(num)) return '';
    return new Intl.NumberFormat('en-US', {
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
    }).format(num);
}

/* Handle Event */
//region Handle Event
let priceInputElement = document.getElementsByClassName('price-input');
if (priceInputElement.length != 0) {
    Array.from(priceInputElement).forEach((PriceElement) => {
        // Kiểm tra trước khi nhập
        if (PriceElement.value == '') {
            PriceElement.value = 0;
        }
        let inputValue = PriceElement.value.replace(/,/g, '');
        let formattedValue = formatNumberVnd(inputValue);
        PriceElement.value = formattedValue;

        // Sự kiện nhập
        PriceElement.oninput = (e) => {
            if (e.target.value == '') {
                e.target.value = 0;
            }
            inputValue = e.target.value.replace(/,/g, '');
            formattedValue = formatNumberVnd(inputValue);
            e.target.value = formattedValue;
        }
    });
} else {
    console.warn("Could not find any price-input class");
}