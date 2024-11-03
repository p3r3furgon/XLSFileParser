const fileInput = document.getElementById('fileInput');
const uploadButton = document.getElementById('uploadButton');
const fileName = document.getElementById('fileName');
const fileCreationDate = document.getElementById('fileCreationDate');
const fileModificationDate = document.getElementById('fileModificationDate');
const fileSize = document.getElementById('fileSize');
const fileList = document.getElementById('fileList');

fileInput.addEventListener('change', () => {
    const file = fileInput.files[0];
    if (file) {
        fileName.textContent = file.name;
        fileCreationDate.textContent = new Date(file.lastModified).toLocaleString();
        fileModificationDate.textContent = new Date(file.lastModified).toLocaleString(); 
        fileSize.textContent = `${(file.size / 1024).toFixed(2)} KB`;
    }
});

function fetchUploadedFiles() {
    fetch('http://localhost:5000/bankdata/files/')
        .then(response => response.json())
        .then(data => {
            data["files"].forEach(file => {
                const listItem = document.createElement('li');
                listItem.innerHTML = `${file.fileName} - добавлен ${new Date(file.dateAdded).toLocaleString()}`;
                listItem.onclick = () => {
                    window.open(`file_info.html?file=${file.fileName}`, '_blank');
                };
                fileList.appendChild(listItem);
            });
        })
        .catch(error => {
            console.error('Ошибка при получении загруженных файлов:', error);
        });
}

uploadButton.addEventListener('click', () => {
    const file = fileInput.files[0];
    if (file) {
        const formData = new FormData();
        formData.append('file', file);

        fetch('http://localhost:5000/bankdata/', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (response.ok) {
                    const listItem = document.createElement('li');
                    listItem.textContent = `${file.name} - добавлен ${new Date(file.lastModified).toLocaleString()}`;
                    listItem.onclick = () => {
                        window.open(`file_info.html?file=${file.name}`, '_blank');
                    };
                    fileList.prepend(listItem);
                    fileInput.value = '';
                    fileName.textContent = '';
                    fileSize.textContent = '';
                    fileCreationDate.textContent = '';
                    fileModificationDate.textContent = '';
                } else {
                    throw new Error();
                }
            })
            .catch(error => {
                console.error('Ошибка:', error);
                alert('Ошибка при загрузке файла.');
            });
    } else {
        alert('Пожалуйста, выберите файл для загрузки.');
    }
});

window.onload = fetchUploadedFiles;
