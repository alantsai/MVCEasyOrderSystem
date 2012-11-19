
$(function () {
        
        $('.global_navi li:last-child').attr("class", "last");

        $('tr:odd').attr('class', 'odd');

        //For the address for register
        $('#Customer_Address_AddFull').focus(function (e) {
            var addCity = $('#Customer_Address_AddCity').val();
            var addDistrict = $('#Customer_Address_AddDistrict').val();
            $(this).val(addCity + addDistrict);

        });

        //爲了下單時候地址的部份會把「市/縣」「區」輸入的填到「完整地址」。
        $('#AddFull').focus(function (e) {
            var addCity = $('#AddCity').val();
            var addDistrict = $('#AddDistrict').val();
            $(this).val(addCity + addDistrict);

        });
        
        //AutoComplete
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

        //訂單選擇「使用註冊資料當地址」時候啟動。需要在做一點Narrower selection
        $(':checkbox').change(function () {
            if ($(this).attr('checked')) {
                $('#address').slideUp();
            }
            else {
                $('#address').slideDown();
            }
        });
    });
