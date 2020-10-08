$(".editing-buttons").hide();
$('body').attr("spellcheck", false);

$(".non-editing-buttons").click(function () {
    $(this).hide();
    let $article = $(this).closest(".article");
    $article.addClass("editing");
    $article.find(".editing-buttons").show();
    let $title = $article.find(".article-title");
    let $content = $article.find(".article-content");
    $title.attr("contenteditable", "true");
    $title.attr("data-restore-content", $title.text());
    $content.attr("contenteditable", "true");
    $content.attr("data-restore-content", $content.html());
    $content.text($content.html());
});

$(".editing-buttons").click(function () {
    $(this).hide();
    let $article = $(this).closest(".article");
    $article.removeClass("editing");
    $article.find(".non-editing-buttons").show();
    let $title = $article.find(".article-title");
    let $content = $article.find(".article-content");
    $title.attr("contenteditable", "false");
    $content.attr("contenteditable", "false");
});

$(".abort-changes").click(function (event) {
    let $article = $(this).closest(".article");
    let $title = $article.find(".article-title");
    let $content = $article.find(".article-content");
    $title.text($title.attr("data-restore-content"));
    $content.html($content.attr("data-restore-content"));
});

$(".submit-changes").click(function (event) {
    let $article = $(this).closest(".article");
    let $content = $article.find(".article-content");
    let $title = $article.find(".article-title");
    $content.html($content.text());
    let data = {
        Id: $article.attr("id"),
        Content: $content.html(),
        Title: $title.text()
    };
    $.ajax({
        url: "/api/Articles/Post",
        type: "POST",
        data: { data: JSON.stringify(data) }
    });
});