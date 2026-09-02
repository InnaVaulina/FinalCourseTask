window.getDivInnerText = (element) => {
    console.log("getDivInnerText called", element);
    return element.innerHTML;
};
window.setDivInnerText = (element, text) => {
    console.log("setDivInnerText called", element, text);
    element.innerText = text;
};






window.alignText = (element, align) => {

    const selection = window.getSelection();
    if (selection.rangeCount === 0) return;

    const range = selection.getRangeAt(0);
    if (range.collapsed) return;

    const selectedParagraphs = new Set();
    const paragraphs = element.querySelectorAll('p');
    
    paragraphs.forEach(p => {
        if (
            p.contains(range.startContainer) ||
            p.contains(range.endContainer) ||
            range.intersectsNode(p)
        ) {
            selectedParagraphs.add(p);
        }
    });

    selectedParagraphs.forEach(p => {
        p.style.textAlign = align;
    });
}


window.putList = (element, typeofList) => {

    console.log("putList called", element);
    if (!['ul', 'ol'].includes(typeofList)) {
        console.error('Неверный тип списка');
        return;
    }

    const selection = window.getSelection();
    if (selection.rangeCount === 0) return;

    const range = selection.getRangeAt(0);
    if (range.collapsed) return;

    const selectedParagraphs = new Set();
    const selectedLists = new Set();

    const paragraphs = element.querySelectorAll('p');
    const lists = element.querySelectorAll(typeofList);

    paragraphs.forEach(p => {
        if (
            p.contains(range.startContainer) ||
            p.contains(range.endContainer) ||
            range.intersectsNode(p)
        ) {
            selectedParagraphs.add(p);
        }
    });

    lists.forEach(list => {
        if (
            list.contains(range.startContainer) ||
            list.contains(range.endContainer) ||
            range.intersectsNode(list)
        ) {
            selectedLists.add(list);
        }
    });

    const newFragment = document.createDocumentFragment();
    if (selectedLists.size === 0 && selectedParagraphs.size > 0) {
        const items = new Set();

        selectedParagraphs.forEach(p => {
            const item = document.createElement("li");
            item.innerHTML = p.innerHTML;
            items.add(item);
        });
        const list = document.createElement(typeofList);
        items.forEach(item => {
            list.appendChild(item);
        });
        newFragment.appendChild(list);


        const newRange = document.createRange();
        newRange.setStartBefore(selectedParagraphs.values().next().value);
        newRange.setEndAfter(Array.from(selectedParagraphs).pop());
        newRange.deleteContents();
        newRange.insertNode(newFragment);

    } else if (selectedLists.size > 0) {
        const paragraphs = new Set();
        selectedLists.forEach(list => {
            const listItems = list.querySelectorAll('li');
            listItems.forEach(item => {
                const paragraph = document.createElement("p");
                paragraph.innerHTML = item.innerHTML;
                paragraphs.add(paragraph);
            });
        });
        paragraphs.forEach(p => {
            newFragment.appendChild(p);
        });


        const newRange = document.createRange();
        newRange.setStartBefore(selectedLists.values().next().value);
        newRange.setEndAfter(Array.from(selectedLists).pop());
        newRange.deleteContents();
        newRange.insertNode(newFragment);
    }

    selection.removeAllRanges();
}





window.insertTextStyle = (element, style) => {
    const selection = window.getSelection();
    if (selection.rangeCount === 0) return;

    const range = selection.getRangeAt(0);
    if (range.collapsed) return;

    const commonParent = range.commonAncestorContainer;
    let node = commonParent.nodeType === Node.ELEMENT_NODE
        ? commonParent
        : commonParent.parentNode;
    console.log("insertTextStyle.commonParent", node.tagName);


    const isWrapped = (node) => {
        while (node) {
            if (node.tagName === style.toUpperCase()) {
                const nodeRange = document.createRange();
                nodeRange.selectNodeContents(node);
                return nodeRange.compareBoundaryPoints(Range.START_TO_START, range) <= 0 &&
                    nodeRange.compareBoundaryPoints(Range.END_TO_END, range) >= 0;
            }
            node = node.parentNode;
        }
        return false;
    };

    if (isWrapped(node)) {
        return;
    }

    const fragment = range.extractContents();
    const elements = fragment.querySelectorAll(style);
    if (elements.length == 0) {
        const newtag = document.createElement(style);
        newtag.appendChild(fragment);
        range.insertNode(newtag);
    } else {
        const tempDiv = document.createElement("div");
        tempDiv.appendChild(fragment);

        elements.forEach((styletag) => {
            const parent = styletag.parentNode;
            while (styletag.firstChild) {
                parent.insertBefore(styletag.firstChild, styletag);
            }
            parent.removeChild(styletag);
        });


        tempDiv.normalize();
        const newFragment = document.createDocumentFragment();
        while (tempDiv.firstChild) {
            newFragment.appendChild(tempDiv.firstChild);
        }

        range.deleteContents();
        range.insertNode(newFragment);
    }
    selection.removeAllRanges();
}



window.initializeEditableWithParagraph = (element,htmltext) => {
    if (element.innerHTML.trim() === "") {
        if (htmltext == "") {
            const p = document.createElement("p");
            p.innerHTML = "<br>";
            element.appendChild(p);
            const range = document.createRange();
            range.setStart(p, 0);
            const selection = window.getSelection();
            selection.removeAllRanges();
            selection.addRange(range);
        } else {

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
    }  
}

window.initializeEditableWithContext = (element) => {
    element.addEventListener("contextmenu", (e) => {
        e.preventDefault();
    });
}


window.undoRedo = {
    undoStack: [],
    redoStack: [],

    saveState: function (element) {
        this.undoStack.push(element.innerHTML);
        this.redoStack = []; 
    },

    undo: function (element) {
        if (this.undoStack.length > 1) {
            this.redoStack.push(this.undoStack.pop());
            element.innerHTML = this.undoStack[this.undoStack.length - 1];
        }
    },

    redo: function (element) {
        if (this.redoStack.length > 0) {
            const state = this.redoStack.pop();
            this.undoStack.push(state);
            element.innerHTML = state;
        }
    }
};


window.getCursorPosition = () => {
    return { clientX: event.clientX, clientY: event.clientY };
};

window.showContextMenu = (element, position) => {
    const customMenu = document.getElementById("customMenu");
    customMenu.style.display = "block";
    customMenu.style.left = `${position.clientX}px`;
    customMenu.style.top = `${position.clientY}px`;
}


window.addEventListener("click", () => {
    const customMenu = document.getElementById("customMenu");
    customMenu.style.display = "none";
});






window.cutText = async (element) => {
    const selection = window.getSelection();
    try {
        if (selection.rangeCount > 0) {
            const text = selection.toString();
            await navigator.clipboard.writeText(text);
            const range = selection.getRangeAt(0);
            range.deleteContents();
            range.insertNode(document.createTextNode(''));
        }
        return true;
    }
    catch (err) {
        console.error('Failed to cut: ', err);
        return false;
    }
};




window.copyText = async (element) => {
    const selection = window.getSelection();
    const text = selection.toString();
    try {
        await navigator.clipboard.writeText(text);
        return true;
    }
    catch (err) {
        console.error('Failed to copy: ', err);
        return false;
    }
};




window.pasteText = async (element) => {

    function cleanNode(node) {
        const allowedTags = ["P", "UL", "OL", "LI", "B", "I", "U", "STRIKE"];
        if (node.nodeType === Node.ELEMENT_NODE) {
            const tagName = node.tagName;
            if (!allowedTags.includes(tagName)) {
                const children = Array.from(node.childNodes);
                node.replaceWith(...children);
                children.forEach(cleanNode);
            } else {
                while (node.attributes.length > 0) {
                    node.removeAttribute(node.attributes[0].name);
                }
                Array.from(node.childNodes).forEach(cleanNode);
            }
        }
    }


    function cleanNestedNode(node) {
        const prohibitedTags = ["P", "UL", "OL", "LI"];
        if (node.nodeType === Node.ELEMENT_NODE) {
            const tagName = node.tagName;
            if (prohibitedTags.includes(tagName)) {
                const children = Array.from(node.childNodes);
                node.replaceWith(...children);
                children.forEach(cleanNestedNode);
            } else {
                Array.from(node.childNodes).forEach(cleanNestedNode);
            }
        }
    }

    const isWrapped = (node, element) => {
        const wrappingTags = ["P", "LI"];
        while (node != element) {
            if (wrappingTags.includes(node.tagName)) {
                return node;
            }
            node = node.parentNode;
        }
        return null;
    };


    try {
        const clipboardItems = await navigator.clipboard.read();
        const tempDiv = document.createElement("div");
        for (const item of clipboardItems) {
            if (item.types.includes("text/html")) {
                const htmlBlob = await item.getType("text/html");
                const htmlText = await new Response(htmlBlob).text();
                tempDiv.innerHTML = htmlText;
                break;
            } else if (item.types.includes("text/plain")) {
                const textBlob = await item.getType("text/plain");
                const text = await new Response(textBlob).text();
                tempDiv.textContent = text;
                break;
            }
        }

        cleanNode(tempDiv);

        const fragment = document.createDocumentFragment();
        const innerfragment = document.createDocumentFragment();

        const structTags = ["P", "UL", "OL"];
        while (tempDiv.firstChild) {
            const child = tempDiv.firstChild;
            const tagName = child.tagName;
            if (structTags.includes(tagName)) {
                if (innerfragment.childNodes.length > 0) {
                    const p = document.createElement("p");
                    while (innerfragment.firstChild) {
                        p.appendChild(innerfragment.firstChild);
                    }
                    fragment.appendChild(p);
                }
                fragment.appendChild(child);
            } else {
                innerfragment.appendChild(child);
            }
        }
        if (innerfragment.childNodes.length > 0) {
            const p = document.createElement("p");
            while (innerfragment.firstChild) {
                p.appendChild(innerfragment.firstChild);
            }
            fragment.appendChild(p);
        }


        Array.from(fragment.childNodes).forEach(child => {
            if (child.tagName == "P") {
                Array.from(child.childNodes).forEach(pchild => {
                    cleanNestedNode(pchild);
                });
            }
            if (child.tagName == "OL" || child.tagName == "UL") {
                Array.from(child.childNodes).forEach(listchild => {
                    Array.from(listchild.childNodes).forEach(itemchild => {
                        cleanNestedNode(itemchild);
                    });
                });
            }
        });


        const selection = window.getSelection();
        if (selection.rangeCount > 0) {
            const range = selection.getRangeAt(0);


            const commonParent = range.commonAncestorContainer;
            let node = commonParent.nodeType === Node.ELEMENT_NODE
                ? commonParent
                : commonParent.parentNode;

            const wrappingNode = isWrapped(node, element);

            if (wrappingNode != null) {
                if (node.innerHTML == "" || node.innerHTML == "<br>") {
                    if (wrappingNode.tagName == "LI") {

                        Array.from(fragment.childNodes).forEach(child => {
                            if (child.tagName == "P") {
                                const newElement = document.createElement("li");
                                while (child.firstChild) {
                                    newElement.appendChild(child.firstChild);
                                }
                                child.replaceWith(newElement)
                            }

                            if (child.tagName == "OL" || child.tagName == "UL") {
                                const newfragment = document.createDocumentFragment();
                                while (child.firstChild) {
                                    newfragment.appendChild(child.firstChild);
                                }
                                child.replaceWith(newfragment);
                            }
                        });
                    }

                    const wrappingNodeRange = document.createRange();
                    wrappingNodeRange.selectNode(wrappingNode);
                    wrappingNodeRange.deleteContents();
                    wrappingNodeRange.insertNode(fragment);

                } else {
                    Array.from(fragment.childNodes).forEach(child => {
                        cleanNestedNode(child);
                    });
                    range.deleteContents();
                    range.insertNode(fragment);

                }
            }
            selection.removeAllRanges();

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
        return true;
    }
    catch (err) {
        console.error('Failed to paste: ', err);
        return false;
    }
};

window.initializePasteHandler = function (dotNetHelper) {

    if (window._pasteHandler) {
        document.removeEventListener("paste", window._pasteHandler, true);
    }

    window._pasteHandler = function (event) {
        event.preventDefault();

        const editableRoot = event.target.closest("[contenteditable='true']");
        if (editableRoot) {
            window.pasteText(editableRoot)
                .then(() => dotNetHelper.invokeMethodAsync("HandlePasteEvent"))
                .catch(error => console.error("Ошибка при вставке:", error));
        }
        return false;
    };

    document.addEventListener("paste", window._pasteHandler, true);
};

