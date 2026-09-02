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



window.initializeCallHandler = function (dotNetHelper) {
    var btn = document.getElementById("button");
    btn.addEventListener("click", function () {
        dotNetHelper.invokeMethodAsync("HandleCallEvent");
    });

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

window.fileHelpers = {
    getUrl: (file) => {
        return URL.createObjectURL(file);
    },
    getFileFromInput: function (input) {
        const f = input.files && input.files[0];
        return f;
    },
    getUrlFromInput: function () {
        const input = document.getElementById("inputFile");
        const f = input.files && input.files[0];
        return f ? URL.createObjectURL(f) : null;
    },
    revokeObjectUrl: function (url) {
        if (url) URL.revokeObjectURL(url);
    }
};


window.showFilters = () => {
    const filters = document.getElementById("filterPoints");
    filters.style.display = "block";
    const filtersButton = document.getElementById("filterButton");
    filtersButton.style.display = "none";   
};

window.hideFilters = () => {
    const filters = document.getElementById("filterPoints");
    filters.style.display = "none";
    const filtersButton = document.getElementById("filterButton");
    filtersButton.style.display = "block";
};

