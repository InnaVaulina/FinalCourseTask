window.getHTML = (element) => {
    return element.innerHTML;
};

window.getText = (element) => {
    return element.innerText;
};

window.setDivInnerText = (element, text) => {
    console.log("setDivInnerText called", element, text);
    element.innerText = text;
};



window.initializePasteHandler = function (dotNetHelper) {
    // ”дал€ем предыдущий обработчик (если был), чтобы не добавл€ть дубликаты
    if (window._pasteHandler) {
        document.removeEventListener("paste", window._pasteHandler, true);
    }

    // ќбработчик в фазе capture Ч перехватываем вставку как можно раньше
    window._pasteHandler = function (event) {
        // ѕолностью запрещаем стандартную вставку
        event.preventDefault();
        event.stopImmediatePropagation();

        // ќпционально Ч уведомл€ем .NET о том, что вставка была заблокирована
        const editableRoot = event.target.closest("[contenteditable='true']");
        if (editableRoot && dotNetHelper && typeof dotNetHelper.invokeMethodAsync === "function") {
            // –еализуйте в .NET метод HandlePasteBlocked при необходимости
            dotNetHelper.invokeMethodAsync("HandlePasteBlocked").catch(() => { /* игнорировать ошибки */ });
        }

        return false;
    };

    document.addEventListener("paste", window._pasteHandler, true);
};


window.initializeEditableWithParagraph = (element, htmltext) => {
    if (htmltext == "") {
        element.innerHTML = "";
        const p = document.createElement("p");
        p.innerHTML = "<br>";
        element.appendChild(p);
        const range = document.createRange();
        range.setStart(p, 0);
        const selection = window.getSelection();
        selection.removeAllRanges();
        selection.addRange(range);
    }
    else {
        element.innerHTML = htmltext;
        if (element.lastChild.tagName != "P") {
            const p = document.createElement("p");
            p.innerHTML = "<br>";
            element.appendChild(p);
            const range = document.createRange();
            range.setStart(p, 0);
            const selection = window.getSelection();
            selection.removeAllRanges();
            selection.addRange(range);
        }
    }
};

window.SetFocusToHTMLParagraph = (element) => {
    if (!element.lastChild)
    {
        const p = document.createElement("p");
        p.innerHTML = "<br>";
        element.appendChild(p);
    }
    if (element.lastChild.tagName == "P") {
        const p = element.lastChild;
        if (p.innerHTML == "<br>")
        {
            const range = document.createRange();
            range.setStart(p, 0);
            const selection = window.getSelection();
            selection.removeAllRanges();
            selection.addRange(range);
        }
    }
};

window.SetFocusToHTMLh1 = (element) => {
    if (!element.lastChild) return;
    if (element.lastChild.tagName == "H1") {
        const h1 = element.lastChild;
        if (h1.innerText == "")
        {
            h1.innerText = "\u200B";
            const range = document.createRange();
            range.setStart(h1, 0);
            const selection = window.getSelection();
            selection.removeAllRanges();
            selection.addRange(range);
        }
    }
};

window.SetFocusToHTMLSpan = (element) => {
    if (!element.lastChild) return;
    if (element.lastChild.tagName == "SPAN") {
        const span = element.lastChild;
        if (span.innerText == "") {
            span.innerText = "\u200B";
            const range = document.createRange();
            range.setStart(span, 0);
            const selection = window.getSelection();
            selection.removeAllRanges();
            selection.addRange(range);            
        }
    }
};