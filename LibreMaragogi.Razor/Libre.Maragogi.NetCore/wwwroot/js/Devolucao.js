$(document).ready(function () {
    $('#usuarios').select2({
        placeholder: 'Selecione Um Usuario',
        language: "pt-BR"
    });

    $('#livros').select2({
        placeholder: 'Selecione Livro(s)',
        language: "pt-BR"
    });

    $("#usuarios").change(function () {
        $('#livros').val(null).trigger('change');

        Url = '/api/Livros/' + $("#usuarios").val();
        request = $.ajax({
            type: 'GET',
            url: Url,
            contentType: "application/json",
            dataType: 'json'
        });

        request.done((data) => {
            $('#livros').empty();

            $.each(data, function (i, item) {
                $('#livros').append($("<option></option>")
                    .attr("value", item.id)
                    .text(item.text));
            });
        });
    });


});