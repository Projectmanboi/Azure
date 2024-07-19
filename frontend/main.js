window.addEventListener('DOMContentLoaded', (event) => {
    console.log('DOM fully loaded and parsed');
    getVisitCount();
});

const functionApi = 'http://localhost:7071/api/GetResumeCounter';

const getVisitCount = () => {
    console.log('Calling API:', functionApi);
    fetch(functionApi)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok ' + response.statusText);
            }
            return response.json();
        })
        .then(data => {
            console.log('API response JSON:', data);
            const count = data.count;
            const counterElement = document.getElementById("counter");
            if (counterElement) {
                counterElement.innerText = count;
                console.log('Counter updated to:', count);
            } else {
                console.error('Element with ID "counter" not found in the DOM.');
            }
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
};