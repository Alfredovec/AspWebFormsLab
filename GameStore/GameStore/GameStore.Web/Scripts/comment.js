$(document).ready(function () {
    var comments = {};
    var getComments = function () {
        $.ajax({
            url: "/api/comments?gameId=" + $('#helpers .id').text(),
            dataType: "json",
            success: function (data) {
                if (!compareComments(comments, data)) {
                    comments = data;
                    $('#comments').html('');
                    $('#comments').html(renderComments(data, null, "treeCSS"));
                    bindActions();
                    console.log(data);
                }
                setTimeout(getComments, 1000);
            }
        });
    };
    getComments();
    $('#createComment').click(function (e) {
        e.preventDefault();
        var form = $('#newComment form');
        if (form.valid()) {
            $.ajax({
                url: "/api/comments?gameId=" + $('#helpers .id').text() + "&quoteId=" + ($('#quoteId').length == 0 ? '-1' : $('#quoteId').val()) + "&parentId=" + ($('#parentId').length == 0 ? '-1' : $('#parentId').val()),
                data: form.serialize(),
                method: "PUT",
                success: function (data, status) {
                    console.log(data);
                    console.log(status);
                    $('#quoteId').remove();
                    $('#parentId').remove();
                    $('#Body').val('');
                }
            });
        }
    });
});


function bindActions() {
    var arr = [
        {
            formId: '#removeParent',
            client: "answer",
            id: "parentId"
        },
        {
            formId: '#quoteComment',
            client: "quote",
            id: "quoteId"
        }];

    for (var i = 0; i < arr.length; i++) {
        $(arr[i].formId).hide();
        var item = arr[i];
        $('.treeCSS .' + item.client).click(function (e) {
            invokeAction($(this), arr);
        });
    }
}

function renderComments(coms, parent, styleName) {
    var ul = $('<ul class="' + styleName + '"></ul>');
    for (var i = 0; i < coms.length; i++) {
        var li = $('<li></li>');
        var commentDiv = $('<div class="comment" id="' + coms[i].Id + '"></div>');
        commentDiv.append('<div class="author">' + coms[i].Name + '</div>');
        var commentBody = $('<div class="body"></div>');
        if (coms[i].IsDeleted) {
            commentBody.text('<i>' + $('#helpers .deleted').text() + '</i>');
        } else {
            if (coms[i].QuoteText != null && coms[i].QuoteText.length != 0) {
                commentBody.append('<div class="text-quote">' + coms[i].QuoteName + ' say: <quote>' + coms[i].QuoteText + '</quote></div>');
            }
            if (parent != null) {
                commentBody.append('<span><a href="#' + parent.Id + '">[' + parent.Name + ']</a>, </span>');
            }
            commentBody.append(coms[i].Body);
            commentBody.append('<div class="action"><div commentid="' + coms[i].Id + '" commentauthor="' + coms[i].Name + '" class="answer">' + $('#helpers .answer').text() + '</div> | <div commentid="' + coms[i].Id + '" commentauthor="' + coms[i].Name + '" class="quote">' + $('#helpers .quote').text() + '</div></div>');
        }
        commentDiv.append(commentBody);
        li.append(commentDiv);
        li.append(renderComments(coms[i].Children, coms[i], ""));
        ul.append(li);
    }
    return ul;
}

function compareComments(com1, com2) {
    if (com1 == null && com2 == null) {
        return true;
    }
    if (com1 == null || com2 == null || com1.length != com2.length) {
        return false;
    }
    for (var i = 0; i < com1.length; i++) {
        if (com1[i].Id != com2[i].Id || !compareComments(com1[i].Children, com2[i].Children)) {
            return false;
        }
    }
    return true;
}

function invokeAction(item, arr) {
    var i = 0;
    var selectedClass = item.attr('class');
    for (; i < arr.length; i++) {
        if (arr[i].client == selectedClass)
            break;
    }
    var id = item.attr('commentid');
    console.log(selectedClass);
    var name = item.attr('commentAuthor');
    $('#' + arr[i].id).remove();
    $('<input>').attr({
        type: 'hidden',
        id: arr[i].id,
        name: arr[i].id,
        value: id
    }).appendTo('#newComment form');
    $(arr[i].formId + ' .name').html('<a href="#' + id + '">[' + name + ']</a>');
    $(arr[i].formId).show();
    $(arr[i].formId + ' .remove').click(function (e) {
        $('#' + arr[i].id).remove();
        $(arr[i].formId).hide();
    });
}