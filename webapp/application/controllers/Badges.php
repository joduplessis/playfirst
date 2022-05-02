<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Badges extends CI_Controller {

	function __construct() {
		parent::__construct();
	}

	public function index() {
		$this->load->model('Badge_model');
		$this->load->model('Node_model');
		$this->load->model('User_model');
		$this->load->model('Activity_model');

		$output = $this->Badge_model->get_all();
		$user_output = $this->User_model->get_all_badges();

		foreach ($output as $badge) {
			// Nodes
			if ($this->Node_model->get_one($badge->node)!=null) {
				$badge->{"node_title"} = $this->Node_model->get_one($badge->node)[0]->title;
			} else {
				$badge->{"node_title"} = "";
			}

			// User badge count
			$totalAmountOfUsersWithThisBadge = $this->Activity_model->get_type_count_all_for_target("badge", $badge->id);

			$badge->{"badge_count"} = $totalAmountOfUsersWithThisBadge;
			$badge->{"badge_count_percent"} = round($totalAmountOfUsersWithThisBadge/$this->Activity_model->get_type_count_all("badge")*100,2);
    }

		$data["badges"] = $output;
		$data["heading"] = "Badges";

		$this->load->view('header');
		$this->load->view('navigation');
		$this->load->view('badges', $data);
		$this->load->view('footer');
	}

	public function badge_add() {
    $this->load->model('Badge_model');

		$title = $this->input->post('title');

    $this->Badge_model->add($title);

		redirect('badges?success=true', 'location');
  }

	public function badge_delete() {
		$id = $this->uri->segment(3);
    $this->load->model('Badge_model');
		$this->Badge_model->delete($id);

		redirect('badges?success="true"', 'location');
  }

	public function badge() {
		$id = $this->uri->segment(3);

  		$this->load->model('Badge_model');
		$this->load->model('Node_model');

		$output = $this->Badge_model->get_one($id);
		$nodes_output = $this->Node_model->get_all();

		$data["badge"] = $output;
		$data["title"] = $output[0]->title;
		$data["nodes"] = $nodes_output;

		$this->load->view('header', $data);
		$this->load->view('navigation');
		$this->load->view('badge');
		$this->load->view('footer');
	}

	public function update_badge() {
		$this->load->model('Badge_model');

		$data['id'] = $_GET['id'];
		$data['image'] = $_GET['image'];
		$data['title'] = $_GET['title'];
		$data['description'] = $_GET['description'];
		$data['node'] = $_GET['node'];
		$data['points'] = $_GET['points'];

		$this->Badge_model->update($data);
   }




}
