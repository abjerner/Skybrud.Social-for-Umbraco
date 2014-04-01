var skybrud = {};

skybrud.social = {
    
    facebook: {
        
        callback: function (id, info) {

            var p = $('#' + id).addClass('authorized');

            $('.name', p).show().html(info.name + '<span> (' + info.id + ')</span>');
            $('.avatar', p).css('background-image', 'url(https://graph.facebook.com/' + info.id + '/picture?type=small)').show();
            $('.authorize', p).hide();
            $('.clear', p).show();

            $('.input.user-id', p).val(info.id);
            $('.input.user-name', p).val(info.name);
            $('.input.access-token', p).val(info.access_token);

        },

        clear: function (id) {

            var p = $('#' + id).removeClass('authorized');

            $('.name', p).hide();
            $('.avatar', p).hide();
            $('.authorize', p).show();
            $('.clear', p).hide();

            $('.input.user-id', p).val('');
            $('.input.user-name', p).val('');
            $('.input.access-token', p).val('');

        }

    },

    instagram: {

        callback: function (id, info) {

            var p = $('#' + id).addClass('authorized');

            $('.name', p).show().html(info.name + '<span> (' + info.id + ')</span>');
            $('.avatar', p).css('background-image', 'url(' + info.avatar + ')').show();
            $('.authorize', p).hide();
            $('.clear', p).show();

            $('.input.user-id', p).val(info.id);
            $('.input.user-name', p).val(info.name);
            $('.input.profile-picture', p).val(info.avatar);
            $('.input.access-token', p).val(info.access_token);

        },

        clear: function (id) {

            var p = $('#' + id).removeClass('authorized');

            $('.name', p).hide();
            $('.avatar', p).hide();
            $('.authorize', p).show();
            $('.clear', p).hide();

            $('.input.user-id', p).val('');
            $('.input.user-name', p).val('');
            $('.input.profile-picture', p).val('');
            $('.input.access-token', p).val('');

        }

    },

    twitter: {

        callback: function (id, info) {

            var p = $('#' + id).addClass('authorized');

            if (info.Name) {
                $('.name', p).show().html(info.Name + "<span> (" + info.ScreenName + " / " + info.UserId + ")</span>");
            } else {
                $('.name', p).show().html(info.ScreenName + "<span> (" + info.UserId + ")</span>");
            }

            $('.avatar', p).css('background-image', 'url(' + info.Avatar + ')').show();
            $('.authorize', p).hide();
            $('.clear', p).show();

            $('.input.userid', p).val(info.UserId);
            $('.input.screenname', p).val(info.ScreenName);
            $('.input.name', p).val(info.Name);
            $('.input.avatar', p).val(info.Avatar);
            $('.input.consumerkey', p).val(info.ConsumerKey);
            $('.input.consumersecret', p).val(info.ConsumerSecret);
            $('.input.accesstoken', p).val(info.AccessToken);
            $('.input.accesstokensecret', p).val(info.AccessTokenSecret);

        },

        clear: function (id) {

            var p = $('#' + id).removeClass('authorized');

            console.log(p.html());

            $('.name', p).hide();
            $('.avatar', p).hide();
            $('.authorize', p).show();
            $('.clear', p).hide();
            
            $('.input.userid', p).val('');
            $('.input.screenname', p).val('');
            $('.input.name', p).val('');
            $('.input.avatar', p).val('');
            $('.input.consumerkey', p).val('');
            $('.input.consumersecret', p).val('');
            $('.input.accesstoken', p).val('');
            $('.input.accesstokensecret', p).val('');

        }

    },

    facebookOAuthCallback: function (id, info) {
        skybrud.social.facebook.callback(id, info);
    },

    clearFacebookOAuth: function (id) {
        skybrud.social.facebook.clear(id);
    }

};