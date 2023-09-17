import { getDocument, GlobalWorkerOptions, PDFDocumentProxy } from "pdfjs-dist";

export module viewer {
    export function init(): void {
        GlobalWorkerOptions.workerSrc = window.location.origin + "/js/pdf.worker.min.js";
    }

    function renderPageToCanvas(pdf: PDFDocumentProxy, pageNumber: number, canvas: HTMLCanvasElement) {
        pdf.getPage(pageNumber).then(function (page) {
            console.log("Page loaded");

            let scale = 1.5;
            let viewport = page.getViewport({ scale: scale });

            // Prepare canvas using PDF page dimensions
            canvas.height = viewport.height;
            canvas.width = viewport.width;

            // Render PDF page into canvas context
            let context = canvas.getContext("2d");
            let renderContext = {
                canvasContext: context,
                viewport: viewport,
            };
            let renderTask = page.render(renderContext);
            renderTask.promise.then(function () {
                console.log("Page rendered");
            });
        });
    }

    export function load(pagesContainerId: string, file: string, templateClassesElementId: string): void {
        init();
        let pagesContainer = document.getElementById(pagesContainerId);
        let templateClasses = document.getElementById(templateClassesElementId).className;

        let loadingTask = getDocument(file);

        loadingTask.promise.then(
            function (pdf) {
                console.log("PDF loaded");

                for (let pageNumber = 1; pageNumber <= pdf.numPages; pageNumber++) {
                    let canvas = document.createElement("canvas") as HTMLCanvasElement;
                    canvas.id = `page-${pageNumber}`;
                    if (templateClasses !== undefined) {
                        canvas.classList.add(...templateClasses.split(" "));
                    }
                    pagesContainer.appendChild(canvas);

                    renderPageToCanvas(pdf, pageNumber, canvas);
                }
            },
            function (reason) {
                // PDF loading error
                console.error(reason);
            }
        );
    }
}
