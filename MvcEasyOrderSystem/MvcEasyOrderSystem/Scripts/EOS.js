
    $(function () {
           
        //For the address for register
        $('#Customer_Address_AddFull').focus(function (e) {
            var addCity = $('#Customer_Address_AddCity').val();
            var addDistrict = $('#Customer_Address_AddDistrict').val();
            $(this).val(addCity + addDistrict);

        });

        $(':input[data-autocomplete]').each(function () {
            $(this).autocomplete({ source: $(this).attr('data-autocomplete') });
        });

        /*For proced to checkout*/
        $('#CollectionMethodId').change(function () {
            var value = $('#CollectionMethodId :selected').text();

            if (value == "送") {
                $('#deliveryOrder').show();
            }
            else {
                $('#deliveryOrder').hide();
            }
        });

        $(':checkbox').change(function () {
            if ($(this).attr('checked')) {
                $('#address').slideUp();
            }
            else {
                $('#address').slideDown();
            }
        });

        $('#AddFull').focus(function (e) {
            var addCity = $('#AddCity').val();
            var addDistrict = $('#AddDistrict').val();
            $(this).val(addCity + addDistrict);

        });
    });
