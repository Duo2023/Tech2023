import { getDocument, GlobalWorkerOptions } from "pdfjs-dist";

export module viewer {
    export function load(canvasId: string, file: string): void {
        // TODO: Fix worker src
        GlobalWorkerOptions.workerSrc = "./pdf.worker.min.js";
        var loadingTask = getDocument(file);

        loadingTask.promise.then(
            function (pdf) {
                console.log("PDF loaded");

                // Fetch the first page
                var pageNumber = 1;
                pdf.getPage(pageNumber).then(function (page) {
                    console.log("Page loaded");

                    var scale = 1.5;
                    var viewport = page.getViewport({ scale: scale });

                    // Prepare canvas using PDF page dimensions
                    var canvas = document.getElementById(canvasId) as HTMLCanvasElement;

                    var context = canvas.getContext("2d");

                    canvas.height = viewport.height;
                    canvas.width = viewport.width;

                    // Render PDF page into canvas context
                    var renderContext = {
                        canvasContext: context,
                        viewport: viewport,
                    };
                    var renderTask = page.render(renderContext);
                    renderTask.promise.then(function () {
                        console.log("Page rendered");
                    });
                });
            },
            function (reason) {
                // PDF loading error
                console.error(reason);
            }
        );
    }
}
