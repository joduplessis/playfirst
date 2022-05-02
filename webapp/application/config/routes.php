<?php
defined('BASEPATH') OR exit('No direct script access allowed');

// Web app routes

$route['404_override'] = '';
$route['translate_uri_dashes'] = FALSE;
$route['default_controller'] = 'management/login';
$route['image/thumbnail']['get'] = 'image/thumbnail';

$route['dashboard']['get'] = 'dashboard';

$route['guides']['get'] = 'guides';
$route['guides/guide_add']['post'] = 'guides/guide_add';
$route['guides/guide']['get'] = 'guides/guide';
$route['guides/guide_delete']['get'] = 'guides/guide_delete';
$route['guides/guide_update']['get'] = 'guides/guide_update';

$route['paths']['get'] = 'paths';
$route['paths/add_path']['post'] = 'paths/add_path';
$route['paths/add_node']['post'] = 'paths/add_node';
$route['paths/update_path']['post'] = 'paths/update_path';
$route['paths/update_node']['post'] = 'paths/update_node';
$route['paths/delete_path']['get'] = 'paths/delete_path';
$route['paths/delete_node']['get'] = 'paths/delete_node';
$route['paths/path']['get'] = 'paths/path';
$route['paths/path_node/(:any)/(:id)']['get'] = 'paths/path_node';

$route['worlds']['get'] = 'worlds';
$route['worlds/world_add']['post'] = 'worlds/world_add';
$route['worlds/world']['get'] = 'worlds/world';
$route['worlds/world_delete']['get'] = 'worlds/world_delete';
$route['worlds/world_update']['post'] = 'worlds/world_update';

$route['badges']['get'] = 'badges';
$route['badges/badge_add']['post'] = 'badges/badge_add';
$route['badges/badge']['get'] = 'badges/badge';
$route['badges/badge_delete']['get'] = 'badges/badge_delete';
$route['badges/update_badge']['post'] = 'badges/update_badge';

$route['players']['get'] = 'players';

$route['management/logout']['get'] = 'management/logout';
$route['management/login']['get'] = 'management/login';
$route['management/login/do']['post'] = 'management/login_do';

$route['assets']['get'] = 'assets';
$route['assets/asset']['get'] = 'assets/asset';
$route['assets/update']['post'] = 'assets/update';

// API routes

$route['api/file_upload']['post'] = 'api/file_upload';

$route['api/player/login']['post'] = 'api/login';
$route['api/player/update']['post'] = 'api/login_update';

$route['api/mwo/(:num)']['get'] = 'api/mwo';
$route['api/mwo/single/(:num)']['get'] = 'api/mwo_single';
$route['api/mwo/add']['post'] = 'api/mwo_add';
$route['api/mwo/update']['post'] = 'api/mwo_update';
$route['api/mwo/delete']['post'] = 'api/mwo_delete';

$route['api/library/badges']['get'] = 'api/library_badges';
$route['api/library/guides']['get'] = 'api/library_guides';
$route['api/library/nodes']['get'] = 'api/library_nodes';
$route['api/library/assets']['get'] = 'api/library_assets';
$route['api/library/paths']['get'] = 'api/library_paths';
$route['api/library/images']['get'] = 'api/library_images';
$route['api/library/worlds']['get'] = 'api/library_worlds';

$route['api/worlds']['get'] = 'api/worlds';
$route['api/worlds/get/(:num)']['get'] = 'api/worlds_get';
$route['api/worlds/start']['get'] = 'api/worlds_get_start';

$route['api/nodes/content_mixed/(:num)']['get'] = 'api/nodes_content_mixed';
$route['api/nodes/content_choices/(:num)']['get'] = 'api/nodes_content_choices';
$route['api/nodes/content_images/(:num)']['get'] = 'api/nodes_content_images';
$route['api/nodes/content_video/(:num)']['get'] = 'api/nodes_content_video';
$route['api/nodes/content_§video_player/(:num)']['get'] = 'api/nodes_content_video_player';
$route['api/nodes/get/(:num)']['get'] = 'api/nodes_get';
$route['api/nodes/get_all']['get'] = 'api/nodes_get_all';

$route['api/badges/get/(:num)']['get'] = 'api/badges_get';

$route['api/guides/get']['get'] = 'api/guides_get';
$route['api/guides/get_all']['get'] = 'api/guides_get_all';

$route['api/leaderboard']['get'] = 'api/leaderboard';
$route['api/leaderboard/leader']['get'] = 'api/leaderboard_leader';
$route['api/leaderboard/position/:(num)']['get'] = 'api/leaderboard_position';

$route['api/activity']['get'] = 'api/activity';
$route['api/activity/log']['post'] = 'api/activity_log';
