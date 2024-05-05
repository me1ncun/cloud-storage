// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var newFolderBtn = document.getElementById("newFolderBtn");
var input = document.getElementById("folderName");
var create = document.getElementById("createFolder");

newFolderBtn.onclick = function() {
    // Проверяем, если поле ввода уже открыто и не пустое, то не показываем его снова
    if (input.style.display === "block" && input.value.trim() !== "") {
        return;
    }

    input.style.display = "block";
    create.style.display = "block";
    // Скрываем кнопку "New folder", чтобы предотвратить повторное нажатие
    newFolderBtn.style.display = "none";
}

// Добавляем обработчик для скрытия кнопки "New folder", если поле ввода пустое после потери фокуса
input.addEventListener('blur', function() {
    if (this.value.trim() === "") {
        input.style.display = "none";
        create.style.display = "none";
        newFolderBtn.style.display = "block";
    }
});

var newFileBtn = document.getElementById("newFileBtn");
var dropZone = document.querySelector(".drop-zone");
var createFile = document.getElementById("createFile");

newFileBtn.onclick = function() {
    // Проверяем, если поле ввода уже открыто и не пустое, то не показываем его снова
    if (dropZone.style.display === "block") {
        return;
    }

    dropZone.style.display = "block";
    createFile.style.display = "block";
    // Скрываем кнопку "New folder", чтобы предотвратить повторное нажатие
    newFileBtn.style.display = "none";
}

document.querySelectorAll(".drop-zone__input").forEach((inputElement) => {
    const dropZoneElement = inputElement.closest(".drop-zone");

    dropZoneElement.addEventListener("click", (e) => {
        inputElement.click();
    });

    inputElement.addEventListener("change", (e) => {
        if (inputElement.files.length) {
            updateThumbnail(dropZoneElement, inputElement.files[0]);
        }
    });

    dropZoneElement.addEventListener("dragover", (e) => {
        e.preventDefault();
        dropZoneElement.classList.add("drop-zone--over");
    });

    ["dragleave", "dragend"].forEach((type) => {
        dropZoneElement.addEventListener(type, (e) => {
            dropZoneElement.classList.remove("drop-zone--over");
        });
    });

    dropZoneElement.addEventListener("drop", (e) => {
        e.preventDefault();

        if (e.dataTransfer.files.length) {
            inputElement.files = e.dataTransfer.files;
            updateThumbnail(dropZoneElement, e.dataTransfer.files[0]);
        }

        dropZoneElement.classList.remove("drop-zone--over");
    });
});
function showRenameInput(button) {
    var newNameInput = button.parentNode.querySelector('.new-name');
    var okButton = button.parentNode.querySelector('.okButton');
    if(newNameInput.style.display === 'block' && newNameInput.value.trim() !== ''){
        return;
    }
    else {
        button.style.display = 'none';
        newNameInput.style.display = 'block';
        okButton.style.display = 'block';
    }
}