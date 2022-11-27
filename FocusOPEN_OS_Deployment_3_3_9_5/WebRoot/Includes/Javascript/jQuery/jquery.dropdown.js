$(function() {

	$("ul.dropdown li").hover(function() {

		$(this).addClass("hover");
		$('ul:first', this).css('visibility', 'visible');

	}, function() {

		$(this).removeClass("hover");
		$('ul:first', this).css('visibility', 'hidden');

	});

	$("a[href='#']").click(function() {
		return false;
	});
	
	$("ul.sub_menu:empty").remove();

	$("ul.dropdown li ul li:has(ul)").find("a:first").append(" &raquo; ");

});