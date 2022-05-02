<?php
header('Access-Control-Allow-Origin: *');
defined('BASEPATH') OR exit('No direct script access allowed');
date_default_timezone_set('Africa/Johannesburg');

class Api extends CI_Controller {
	public function __construct() {
		parent::__construct();
	}

	public function login() {
		$this->load->model('User_model');
		$output = $this->User_model->login();
		$this->output->set_content_type('application/json')->set_output(json_encode($output));
	}

	public function login_update() {
		$this->load->model('User_model');
		$output = $this->User_model->update();
		$this->output->set_content_type('application/json')->set_output(json_encode($output));
	}

	public function activity() {
		$this->load->model('Activity_model');
		$this->load->model('Badge_model');
		$this->load->model('User_model');
		$this->load->model('Node_model');
		$this->load->model('Path_model');
		$this->load->model('World_model');
		$this->load->model('Mwo_model');
		$this->load->model('Asset_model');

		$this->load->helper('time');

		$output = $this->Activity_model->get_last_twenty();

	  foreach ($output as $active) {
	    // Change the image
	    $user = $this->User_model->get_one($active->user);
	    $active->{"image"} = base_url()."avatars/".$user[0]->avatar.".png";
	    $active->{"playname"} = $user[0]->playname;
	    $active->{"ago"} = time_elapsed_string('@'.$active->created_at);

	    switch ($active->type) {
	      case "badge":
	        $active->{"status"} = "... has just recieved the '".$this->Badge_model->get_one($active->target)[0]->title."' badge";
	        break;
	      case "node_complete":
	        $active->{"status"} = "... has just completed '".$this->Node_model->get_one($active->target)[0]->title."'";
	        break;
	      case "node_open":
	        $active->{"status"} = "... has just started '".$this->Node_model->get_one($active->target)[0]->title."'";
	        break;
	      case "paths_activate":
	        $active->{"status"} = "... has just activated '".$this->Path_model->get_one($active->target)[0]->title."'";
	        break;
	      case "path_activate":
	        $active->{"status"} = "... has just activated '".$this->Path_model->get_one($active->target)[0]->title."'";
	        break;
	      case "world_enter":
	        $active->{"status"} = "... has just entered '".$this->World_model->get_one($active->target)[0]->title."'";
	        break;
	      case "login":
	        $active->{"status"} = "... has just logged in";
	        break;
				case "logout":
	        $active->{"status"} = "... has just logged out";
	        break;
	      case "like":
	        $active->{"status"} = "... has just liked '".$this->Mwo_model->get_one_object($active->target)[0]->name."'";
	        break;
	    }
	  }

		$this->output->set_content_type('application/json')->set_output(json_encode($output, JSON_UNESCAPED_SLASHES));
	}

  public function activity_log() {
    $this->load->model('Activity_model');
    $this->Activity_model->log();
  }

	public function mwo() {
		$id = $this->uri->segment(3);
		$this->load->model('Mwo_model');
		$output = $this->Mwo_model->get_one($id);
		$this->output->set_content_type('application/json')->set_output(json_encode($output));
	}

	public function mwo_single() {
		$id = $this->uri->segment(3);
		$this->load->model('Mwo_model');
		$output = $this->Mwo_model->get_one_object($id);
		$this->output->set_content_type('application/json')->set_output(json_encode($output));
	}

	public function mwo_add() {
    $this->load->model('Mwo_model');
    $output = $this->Mwo_model->add();
		$this->output->set_content_type('application/json')->set_output($output);
  }

	public function mwo_update() {
    $this->load->model('Mwo_model');
    $output = $this->Mwo_model->update();
		$this->output->set_content_type('application/json')->set_output($output);
  }

	public function mwo_delete() {
    $this->load->model('Mwo_model');
    $output = $this->Mwo_model->delete();
		$this->output->set_content_type('application/json')->set_output($output);
  }

	public function library_badges() {
		$this->load->model('Badge_model');
		$output = $this->Badge_model->get_all();

		$this->output->set_content_type('application/json')->set_output(json_encode($output, JSON_UNESCAPED_SLASHES));
	}

	public function library_guides() {
		$this->load->model('Guide_model');
		$output = $this->Guide_model->get_all();

		$this->output->set_content_type('application/json')->set_output(json_encode($output, JSON_UNESCAPED_SLASHES));
	}

	public function library_worlds() {
		$this->load->model('World_model');
		$output = $this->World_model->get_all();

		$this->output->set_content_type('application/json')->set_output(json_encode($output, JSON_UNESCAPED_SLASHES));
	}

	public function library_nodes() {
		$this->load->model('Node_model');
		$this->load->helper('sorting');

		$output = $this->Node_model->get_all();

		$this->output->set_content_type('application/json')->set_output(json_encode($output));
	}

	public function library_assets() {
		$this->load->model('Asset_model');
		$output = $this->Asset_model->get_all();
		$this->output->set_content_type('application/json')->set_output(json_encode($output));
	}

	public function library_paths() {
		$this->load->model('Path_model');
		$output = $this->Path_model->get_all();

		$this->output->set_content_type('application/json')->set_output(json_encode($output, JSON_UNESCAPED_SLASHES));
	}

  public function library_images() {
    $this->load->model('Path_model');
    $this->load->model('Badge_model');
    $this->load->model('Asset_model');
    $this->load->model('Node_model');

    $outputBadge = $this->Badge_model->get_all();
    $outputAsset = $this->Asset_model->get_all();
    $outputNode = $this->Node_model->get_all();

    $images = [];

    foreach ($outputBadge as $badge) {
      array_push($images, $badge->image);
    }

    foreach ($outputAsset as $asset) {
      array_push($images, $asset->image);
    }

    foreach ($outputNode as $node) {
			if ($node->type=="images") {
	      $array = explode(",", $node->content);
	      foreach ($array as $a) {
	        array_push($images, $a);
	      }
			}
    }

    $images = array_unique($images, SORT_REGULAR);

    $this->output->set_content_type('application/json')->set_output(json_encode($images, JSON_UNESCAPED_SLASHES));



  }

	public function nodes_content_mixed() {
		$id = $this->uri->segment(4);
    $this->load->model('Node_model');
		$output = $this->Node_model->get_content($id);
		$data = array('content' => $output[0]->content);
		$this->load->view('nodes/content_mixed', $data);
	}

	public function nodes_content_choices() {
		$id = $this->uri->segment(4);
    $this->load->model('Node_model');
		$output = $this->Node_model->get_content($id);
		$data = array('content' => $output[0]->content);
		$this->output->set_content_type('application/json')->set_output(json_encode($data));
	}

	public function nodes_content_video() {
		$id = $this->uri->segment(4);
    $this->load->model('Node_model');
		$output = $this->Node_model->get_content($id);
		$data = array('content' => $output[0]->content);
		$this->output->set_content_type('application/json')->set_output(json_encode($data, JSON_UNESCAPED_SLASHES));
	}

	public function nodes_content_video_player() {
		$id = $this->uri->segment(4);
    $this->load->model('Node_model');
		$output = $this->Node_model->get_content($id);
		$data = array('content' => $output[0]->content);
		$this->load->view('nodes/content_video', $data);
	}

	public function nodes_content_images() {
		$id = $this->uri->segment(4);
    $this->load->model('Node_model');
		$output = $this->Node_model->get_content($id);
		$data = array('content' => $output[0]->content);
		$this->output->set_content_type('application/json')->set_output(json_encode($data, JSON_UNESCAPED_SLASHES));
	}

	public function nodes_get() {
		$id = $this->uri->segment(4);
		$this->load->model('Node_model');
		$output = $this->Node_model->get_one($id);
		$this->output->set_content_type('application/json')->set_output(json_encode($output[0], JSON_UNESCAPED_SLASHES));
	}

	public function nodes_get_all() {
		$this->load->model('Node_model');
		$output = $this->Node_model->get_all();
		$this->output->set_content_type('application/json')->set_output(json_encode($output, JSON_UNESCAPED_SLASHES));
	}

	public function badges_get() {
		$id = $this->uri->segment(3);
		$this->load->model('Badge_model');
		$output = $this->Badge_model->get_one($id);
		$this->output->set_content_type('application/json')->set_output(json_encode($output[0], JSON_UNESCAPED_SLASHES));
	}

	public function guides_get() {
		$id = $this->uri->segment(3);
		$this->load->model('Guide_model');
		$output = $this->Guide_model->get_one($id);
		$this->output->set_content_type('application/json')->set_output(json_encode($output[0], JSON_UNESCAPED_SLASHES));
	}

	public function guides_get_all() {
		$this->load->model('Guide_model');
		$output = $this->Guide_model->get_all();
		$this->output->set_content_type('application/json')->set_output(json_encode($output, JSON_UNESCAPED_SLASHES));
	}

	public function leaderboard() {
		$this->load->model('User_model');
		$this->load->helper('sorting');

		$output = $this->User_model->get_all();

    usort($output, 'compare_points');

    foreach ($output as $user) {
      $user->{"image"} = base_url()."avatars/".$user->avatar.".png";
    }

		$this->output->set_content_type('application/json')->set_output(json_encode($output, JSON_UNESCAPED_SLASHES));
	}

  public function leaderboard_leader() {
		$this->load->model('User_model');
		$this->load->helper('sorting');

		$output = $this->User_model->get_all();

    usort($output, 'compare_points');

		$this->output->set_content_type('application/json')->set_output(json_encode($output[0], JSON_UNESCAPED_SLASHES));
	}

  public function leaderboard_position() {
		$this->load->helper('sorting');

  	$id = $this->uri->segment(3);
    $position = 0;

		$this->load->model('User_model');
		$output = $this->User_model->get_all();

    usort($output, 'compare_points');

    foreach ($output as $user) {
      $position++;

      if ($user->id==$id) {
        break;
      }
    }

		$this->output->set_content_type('application/json')->set_output(json_encode($position, JSON_UNESCAPED_SLASHES));
	}

	public function worlds() {
		$this->load->model('World_model');
		$output = $this->World_model->get_all();
		$this->output->set_content_type('application/json')->set_output(json_encode($output));
	}

	public function worlds_get() {
		$id = $this->uri->segment(3);
    $this->load->model('World_model');
		$output = $this->World_model->get_all();
    $api_output = "";

    foreach ($output as $world) {
      if ($world->id==$id) {
        $api_output = $world;
      }
    }
		$this->output->set_content_type('application/json')->set_output(json_encode($api_output));
	}

	public function worlds_get_start() {
		$this->load->model('World_model');
		$output = $this->World_model->get_all();
    $startId = -1;
    $api_output = "";

    foreach ($output as $world) {
      if ($world->start) {
        $api_output = $world;
      }
    }

		$this->output->set_content_type('application/json')->set_output(json_encode($api_output));
	}

	public function file_upload() {
		$name = $_FILES["file"]["name"];
		$fileType = pathinfo($_FILES["file"]["name"], PATHINFO_EXTENSION);
		$root = "uploads";
		$month = date("m");
		$day = date("d");
		$targetFile = $root."/".$month."/".$day."/".time().".".$fileType;

		if (!file_exists($root.'/'.$month))
		    mkdir($root.'/'.$month, 0777, true);

		if (!file_exists($root.'/'.$month.'/'.$day))
		    mkdir($root.'/'.$month.'/'.$day, 0777, true);

		if (move_uploaded_file($_FILES["file"]["tmp_name"], $targetFile)) {
			echo $targetFile;
		} else {
			echo "failed";
		}
	}
}
