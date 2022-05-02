<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Paths extends CI_Controller {

	function __construct() {
		parent::__construct();

		$this->load->library('session');
	}

	public function index() {
    $this->load->model('Path_model');
		$this->load->model('Node_model');
		$this->load->model('Activity_model');
		$this->load->model('User_model');
		$this->load->helper('user_authentication');

		// Called from our helpers
		check_authenticated_user();

		$paths = $this->Path_model->get_all();
		$allPathActivations = $this->Activity_model->get_all_path_activation_count();

		foreach ($paths as $path) {
			// number of activations
			$activations = $this->Activity_model->get_path_activation_count($path->id);
			$activations_percent = ($activations/$allPathActivations)*100;
			$path->{"activations"} = $activations;
			$path->{"activations_percent"} = round($activations_percent, 2);

			// User image
			$path->user_image = $this->User_model->get_one($path->user)[0]->avatar;

			// Description
      $path->description = substr($path->description, 0, 50). " ...";

			// Nodes
      $path->nodes = $this->Node_model->get_path_nodes($path->id);

			// Nodes completed
			$allNodeCompletionCount = $this->Activity_model->get_all_node_completion_count();
			foreach ($path->nodes as $node) {
				$completions = $this->Activity_model->get_node_completion_count($node->id);
				$completions_percent = ($completions/$allNodeCompletionCount)*100;
				$node->{"completions"} = $completions;
				$node->{"completions_percent"} = round($completions_percent, 2);
			}
    }

		$data["paths"] = $paths;
		$data["heading"] = "Paths";

		$this->load->view('header', $data);
		$this->load->view('navigation');
		$this->load->view('paths');
		$this->load->view('footer');
	}

	public function add_path() {
  	$this->load->model('Path_model');

		$title = $this->input->post('title');

    $this->Path_model->add($title);

		redirect('paths?success=true', 'location');
  }

	public function add_node() {
		$this->load->model('Node_model');

		$pid = $this->input->post('path');
		$title = $this->input->post('title');
		$type = $this->input->post('type');

		$this->Node_model->add($pid, $title, $type);

		redirect('paths/path/'.$pid.'?success=true', 'location');
	}

	public function path() {
		$this->load->model('Node_model');
    $this->load->model('Path_model');
		$this->load->model('Activity_model');
		$this->load->helper('user_authentication');

		// Called from our helpers
		check_authenticated_user();

		$id = $this->uri->segment(3);
		$path = $this->Path_model->get_one($id)[0];
		$nodes = $this->Node_model->get_path_nodes($path->id);

		// Nodes completed
		$allNodeCompletionCount = $this->Activity_model->get_all_node_completion_count();

		foreach ($nodes as $node) {
			$completions = $this->Activity_model->get_node_completion_count($node->id);
			$completions_percent = ($completions/$allNodeCompletionCount)*100;
			$node->{"completions"} = $completions;
			$node->{"completions_percent"} = round($completions_percent, 2);
		}

		$data["nodes"] = $nodes;
		$data["path"] = $path;
		$data["heading"] = $path->title;
		$data["paths"] = $this->Path_model->get_all();
		$data["user"] = $this->session->get_userdata("user")["user"][0]->id;

		$this->load->view('header', $data);
		$this->load->view('navigation');
		$this->load->view('path');
		$this->load->view('footer');
	}

	public function path_node() {
		$id = $this->uri->segment(4);
	  $this->load->model('Node_model');
		$this->load->helper('user_authentication');

		// Called from our helpers
		check_authenticated_user();

		$node = $this->Node_model->get_one($id)[0];

		$data["node"] = $node;
		$data["title"] = $node->title;

		$this->load->view('header', $data);
		$this->load->view('navigation');
		$this->load->view('node');
		$this->load->view('footer');
	}

	public function update_path() {
		$id = $this->input->post('id');

    $this->load->model('Path_model');
    $this->Path_model->update();

		redirect('paths/path/'.$id.'?success=true', 'location');
  }

	public function update_node() {
		$this->load->model('Node_model');

		$data['id'] = $_GET['id'];
		$data['title'] = $_GET['title'];
		$data['content'] = $_GET['content'];
		$data['points'] = $_GET['points'];

		$this->Node_model->update($data);
   }

	public function delete_path() {
		$id = $this->uri->segment(3);

    $this->load->model('Node_model');
		$this->load->model('Path_model');

		$nodes = $this->Node_model->get_path_nodes($id);

		foreach ($nodes as $node) {
    	$this->Node_model->delete($node->id);
    }

		$this->Path_model->delete($id);

		redirect('paths?success=true', 'location');
  }

	public function delete_node() {
		$id = $this->uri->segment(3);

    $this->load->model('Node_model');
		$this->Node_model->delete($id);

		redirect('paths?success=true', 'location');
   }









}
