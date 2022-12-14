/*
Copyright (c) 2003-2010, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	config.toolbar = 'FocusOPENToolbar';

	config.toolbar_FocusOPENToolbar =
    [
        ['Source', '-', 'Maximize', '-', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Scayt'],
        ['Undo', 'Redo', '-', 'Find', 'Replace'],
        '/',
        ['Image', 'Flash', 'Table', 'HorizontalRule', 'SpecialChar', 'PageBreak', '-', 'Outdent', 'Indent', 'Blockquote', '-', 'Link', 'Unlink', 'Anchor', '-', 'SelectAll', 'RemoveFormat'],
        '/',
        ['Styles', 'Format'],
        ['Bold', 'Italic', 'Strike'],
        ['NumberedList', 'BulletedList'],
        ['About']
    ];
};
