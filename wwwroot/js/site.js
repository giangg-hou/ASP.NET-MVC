/*
// Update cart count
function updateCartCount() {
    $.ajax({
        url: '/GioHang/GetCartCount',
        type: 'GET',
        success: function(data) {
            $('#cart-count').text(data.count);
            if (data.count > 0) {
                $('#cart-count').show();
            } else {
                $('#cart-count').hide();
            }
        }
    });
}

// Auto hide alerts after 5 seconds
$(document).ready(function() {
    setTimeout(function() {
        $('.alert').fadeOut('slow');
    }, 5000);
    
    // Update cart count on page load
    if ($('#cart-count').length) {
        updateCartCount();
    }
});

// Confirm delete
function confirmDelete(message) {
    return confirm(message || 'Bạn có chắc muốn xóa?');
}
*/