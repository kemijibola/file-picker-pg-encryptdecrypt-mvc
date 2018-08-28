function Alert() {
}

Alert.prototype.jInfoAlert = function (title, content, theme, backgroundColor)
{
    $.jAlert({
        'title': title,
        'content': content,
        'theme': theme,
        'closeOnClick': true,
        'backgroundColor': backgroundColor,
        'btns': [
            { 'text': 'OK', 'theme': theme }
        ]
    });
}