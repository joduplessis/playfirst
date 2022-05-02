<?php
class Node_model extends CI_Model {

  public $path;
  public $title;
  public $type;
  public $content;
  public $points;

  public function __construct() {
    parent::__construct();
  }

  public function get_all() {
	  $query = $this->db->get('nodes');

	  return $query->result();
  }

  public function get_all_of_type($type) {
    $this->db->where('type', $type);
	  $query = $this->db->get('nodes');
	  return $query->result();
  }

  public function get_one($id) {
	  $this->db->where('id', $id);
	  $query = $this->db->get('nodes');
	  return $query->result();
  }

  public function get_content($id) {
	  $this->db->where('id', $id);
    $query = $this->db->get('nodes');
    return $query->result();
  }

  public function get_path_nodes($id) {
    $this->db->where('path', $id);
    $query = $this->db->get('nodes');
    return $query->result();
  }

  public function delete($id) {
    $query = $this->db->delete('nodes', array('id' => $id));
  }

  public function add($pid, $title, $type) {
    $this->path = $pid;
    $this->title = $title;
    $this->type = $type;
    $this->content = "";
    $this->points = 0;

    $this->db->insert('nodes', $this);
  }

  public function update($data) {
    $this->db->where('id', $data["id"]);
    return $this->db->update('nodes', $data);
  }
}
?>
